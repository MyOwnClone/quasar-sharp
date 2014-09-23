using System;
using System.Collections.Generic;

namespace GruntXProductions.Quasar.VM
{
	public class PeripheralController
	{
		private Dictionary<int, Device> ioPorts = new Dictionary<int, Device>();
		private Emulator host;
		
		public Emulator Host
		{
			get
			{
				return this.host;
			}
		}
		
		public PeripheralController(Emulator host)
		{
			this.host = host;
		}
		
		public bool RequestIOPort(Device dev, int port)
		{
			if(!ioPorts.ContainsKey(port))
			{
				ioPorts[port] = dev;
				return true;
			}
			else
				return false;
		}
		
		public void FreeIOPort(int port)
		{
			ioPorts.Remove(port);
		}
		
		public void Out(int port, uint data)
		{
			if(ioPorts.ContainsKey(port))
				ioPorts[port].RecieveData(port, data);
		}
		
		public uint In(int port)
		{
			if(ioPorts.ContainsKey(port))
				return ioPorts[port].RequestData(port);
			return 0;
		}
		
		
	}
}

