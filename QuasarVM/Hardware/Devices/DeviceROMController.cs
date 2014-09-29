using System;
using System.IO;
using System.Collections.Generic;

namespace GruntXProductions.Quasar.VM
{
	public class DeviceROMController : Device
	{
		class RomImage
		{
			private static uint start = 0xFF9F0000;
			
			private Stream image;
			private uint address;
			private uint limit;
			
			public Stream Image
			{
				get
				{
					return this.image;
				}
			}
			
			public uint Address
			{
				get
				{
					return address;
				}
			}
			
			public uint Limit
			{
				get
				{
					return this.limit;
				}
			}
			
			public RomImage(Stream img)
			{
				this.image = img;
				this.address = start;
				start += (uint)img.Length;
				this.limit = start;
			}
		}
		
		private Stream bootFirmware;
		private byte[] romTable;
		private List<RomImage> images = new List<RomImage>();
		
		public DeviceROMController(Stream bootCode)
		{
			bootFirmware = bootCode;	
		}
		
		public void AddROM(Stream image)
		{
			this.images.Add(new RomImage(image));
		}
		
		public override void Init (Emulator emu)
		{
			emu.Memory.MapRegion(new DeviceMappedRegion(0xFFA00000, 0xFFCFFFFF, null, romReadCallback));
			emu.Memory.MapRegion(new DeviceMappedRegion(0xFF9F0000, 0xFF9FFFFF, null, romTableReadCallback));
			emu.Memory.MapRegion(new DeviceMappedRegion(0xFFD00000, 0xFFDFFFFF, null, bootReadCallback));
			using (MemoryStream ms = new MemoryStream())
			{
				BinaryWriter bw = new BinaryWriter(ms);
				foreach(RomImage img in this.images)
				{
					bw.Write(img.Address);
					bw.Write((uint)img.Image.Length);
					bw.Write(0);
				}
				this.romTable = ms.ToArray();
			}
		}
		
		private void bootReadCallback(uint address, ref byte data)
		{
			uint actualAddress = address & 0xFFFFF;
			bootFirmware.Seek(actualAddress, SeekOrigin.Begin);
			data = (byte)bootFirmware.ReadByte();
		}
		
		private void romTableReadCallback(uint address, ref byte data)
		{
			uint actualAddress = address & 0xFFFFF;
			if(actualAddress < romTable.Length)
				data = romTable[actualAddress];
		}
		
		private void romReadCallback(uint address, ref byte data)
		{
			foreach(RomImage img in this.images)
			{
				if(img.Address <= address && img.Limit > address)
				{
					img.Image.Seek(address - img.Address, SeekOrigin.Begin);
					data = (byte)img.Image.ReadByte();
					return;
				}
			}
		}
	}
}

