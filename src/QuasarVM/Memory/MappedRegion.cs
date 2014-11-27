using System;

namespace GruntXProductions.Quasar.VM
{
	
	public delegate void DataWrittenCallback(uint addr, byte data);
	public delegate void DataReadCallback(uint addr, ref byte data);
	
	public class DeviceMappedRegion
	{
		
		private uint mbase;
		private uint mlimit;
		private DataWrittenCallback writeCallback;
		private DataReadCallback readCallback;
		
		public uint Base
		{
			get
			{
				return this.mbase;
			}
		}
		
		public uint Limit
		{
			get
			{
				return this.mlimit;
			}
		}
		
		public DeviceMappedRegion(uint start, uint end, DataWrittenCallback writeCallback, DataReadCallback readCallback)
		{
			this.mbase = start;
			this.mlimit = end;
			this.writeCallback = writeCallback;
			this.readCallback = readCallback;
		}
		
		public void Write(uint addr, byte data)
		{
			if(this.writeCallback != null)
				this.writeCallback(addr, data);
		}
		
		public byte Read(uint addr)
		{
			byte ret = 0;
			if(this.readCallback != null)
				this.readCallback(addr, ref ret);
			return ret;
		}
	}
}

