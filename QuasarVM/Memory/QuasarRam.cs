using System;
using System.Collections.Generic;

namespace GruntXProductions.Quasar.VM
{
	public class QuasarRam
	{
		private uint limit;
		private Dictionary<uint, MemoryBlock> memoryBlocks = new Dictionary<uint, MemoryBlock>();
		private List<DeviceMappedRegion> mappedMemory = new List<DeviceMappedRegion>();
		private Dictionary<uint, uint> lookasideBuffer = new Dictionary<uint, uint>();
		private uint pageDirectory = 0;
		private bool pagingEnabled = false;
		
		
		public uint Limit	
		{
			get
			{
				return this.limit;
			}
		}
		
		public QuasarRam(uint size)
		{
			this.limit = size;
		}
		
		public byte this[uint index]
		{
			get
			{
				if (isMapped(index))
					return getDeviceMap(index).Read(index);
				else if(index > limit)
					throw new SegmentationFaultException(index);
				uint aligned = (uint)(index & 0xFFFFF000);
				if(memoryBlocks.ContainsKey(aligned))
					return memoryBlocks[aligned][index];
				else
					return 0;
			}
			set	
			{
				if (isMapped(index))
					getDeviceMap(index).Write(index, value);
				else if(index > limit)
					throw new SegmentationFaultException(index);
				else
				{
					uint aligned = (uint)(index & 0xFFFFF000);
					if(memoryBlocks.ContainsKey(aligned))
						memoryBlocks[aligned][index] = value;
					else
					{
						MemoryBlock mb = new MemoryBlock(aligned);
						mb[index] = value;
						memoryBlocks[aligned] = mb;
					}
				}
			}
		}
		
		public void EnablePaging(uint pageDirectory)
		{
			this.pageDirectory = pageDirectory;
			this.pagingEnabled = true;
			this.lookasideBuffer.Clear();
		}
		
		public void DisablePaging()
		{
			this.pagingEnabled = false;
			this.lookasideBuffer.Clear();
		}
		
		public int Write(uint address, int size, int offset, byte[] data, bool physical = false)
		{
			int written;
			for(written = 0; written < size; written++)
			{
				WriteInt8(address++, data[offset + written], physical);
			}
			return written;
			
		}
		
		public int Read(uint address, int size, int offset, byte[] data, bool physical = false)
		{
			int read;
			for(read = 0; read < size; read++)
			{
				data[offset + read] = (byte)ReadInt8(address++, physical);
			}
			return read;
			
		}
		
		public void WriteInt32(uint address, uint i, bool physical = false)
		{
			Write(address, 4, 0, BitConverter.GetBytes(i), physical);
		}
		
		public void WriteInt32(uint address, int i, bool physical = false)
		{
			WriteInt32(address, (uint)i, physical);
		}
		
		public void WriteInt8(uint address, byte b, bool physical = false)
		{
			if(!this.pagingEnabled || physical)
				this[address] = b;
			else
			{
				uint vaddr_pa = address & 0xFFFFF000;
				if(!lookasideBuffer.ContainsKey(vaddr_pa))	
				{
					uint vaddr = address / 0x1000;
					uint table = this.ReadInt32(pageDirectory + (vaddr / 1024), true);
					lookasideBuffer.Add(vaddr_pa, this.ReadInt32(table + 4 * (vaddr % 1024), true));
				
				}
				uint page = lookasideBuffer[vaddr_pa];
				if((page & Page.PRESENT) == 0)
					throw new PageFaultException(address);
				uint phys = (lookasideBuffer[vaddr_pa] & 0xFFFF000) | (address & 0xFFF);
				
				this[phys] = b;
			}
		}
		
		public uint ReadInt32(uint address, bool physical = false)
		{
			byte[] data = new byte[4];
			Read(address, 4, 0, data, physical);
			return BitConverter.ToUInt32(data, 0);
		}
		
		public uint ReadInt8(uint address, bool physical = false)
		{
			if(!this.pagingEnabled || physical)
				return this[address];
			else
			{
				uint vaddr_pa = address & 0xFFFFF000;
				if(!lookasideBuffer.ContainsKey(vaddr_pa))	
				{
					uint vaddr = address / 0x1000;
					uint table = this.ReadInt32(pageDirectory + (vaddr / 1024), true);
					
					lookasideBuffer.Add(vaddr_pa, this.ReadInt32(table + 4 * (vaddr % 1024), true));
				
				}
				uint page = lookasideBuffer[vaddr_pa];
				if((page & Page.PRESENT) == 0)
				{
					throw new PageFaultException(address);
				}
				uint phys = (lookasideBuffer[vaddr_pa] & 0xFFFF000) | (address & 0xFFF);
				uint attr = lookasideBuffer[vaddr_pa] & 0xFFF;
				return this[phys];
			}
		}
		
		public uint GetPagePermissions(uint address)
		{
			uint vaddr_pa = address & 0xFFFFF000;
			if(!lookasideBuffer.ContainsKey(vaddr_pa))	
			{
				uint vaddr = address / 0x1000;
				uint table = this.ReadInt32(pageDirectory + (vaddr / 1024), true);
				lookasideBuffer.Add(vaddr_pa, this.ReadInt32(table + 4 * (vaddr % 1024), true));
			
			}
			uint page = lookasideBuffer[vaddr_pa];
			if((page & Page.PRESENT) == 0)
				throw new PageFaultException(address);
			return lookasideBuffer[vaddr_pa] & 0xFFF;
		}
		
		public void MapRegion(DeviceMappedRegion mmap)
		{
			this.mappedMemory.Add(mmap);
		}
	
		private bool isMapped(uint address)
		{
			for(int i = 0; i < this.mappedMemory.Count; i++)
			{
				if(this.mappedMemory[i].Base <= address && this.mappedMemory[i].Limit > address)
					return true;
			}
			return false;
		}
		
		private DeviceMappedRegion getDeviceMap(uint address)
		{
			for(int i = 0; i < this.mappedMemory.Count; i++)
			{
				if(this.mappedMemory[i].Base <= address && this.mappedMemory[i].Limit > address)
					return this.mappedMemory[i];
			}
			return null;
		}
	}
}

