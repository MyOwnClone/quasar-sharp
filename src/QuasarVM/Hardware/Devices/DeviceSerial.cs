using System;
using System.IO;

namespace GruntXProductions.Quasar.VM
{
	public class DeviceSerial : Device
	{
		private Stream stream;
		private int comPort = 0;
		public DeviceSerial (int comport, Stream str)
		{
			switch(comport)
			{
			case 0:
				comPort = 0x3F8;
				break;
			default:
				throw new Exception("");
			}
			this.stream = str;
		}
		
		public override void Init (Emulator emu)
		{
			emu.PeripheralController.RequestIOPort(this, this.comPort);
		}
		
		public override void RecieveData (int port, uint data)
		{
			stream.WriteByte((byte)data);
		}
		
		public override uint RequestData (int port)
		{
			return (uint)stream.ReadByte();
		}
	}
}

