using System;
using System.IO;

namespace GruntXProductions.Quasar.VM
{
	public class DeviceSerialController : Device
	{
		const byte STATUS_SUCCESS = 1;
		const byte STATUS_EMPTY = 2;
		const byte STATUS_ERR = 4;
		
		private object[] streams = new object[4];
		private byte dataRegister;
		private byte statusRegister;
		private byte selectedPort;

		public void OpenComPort(byte port, Stream strm)
		{
			streams[port] = strm;
		}
		
		public override void Init (Emulator emu)
		{
			emu.PeripheralController.RequestIOPort(this, 0x05);
			emu.PeripheralController.RequestIOPort(this, 0x06);
			emu.PeripheralController.RequestIOPort(this, 0x07);
		}
		
		public override void RecieveData (int port, uint data)
		{
			switch(port) 
			{
			case 0x05:
				this.dataRegister = (byte)data;
				break;
			case 0x06:
				interpretCommand((byte)data);
				break;
			}
		}
		
		public override uint RequestData (int port)
		{
			switch(port) 
			{
			case 0x05:
				return this.dataRegister;
			case 0x07:
				return this.statusRegister;
			}
			return 0;
		}
		
		private void interpretCommand(byte command)
		{
			switch(command)
			{
			case 0:
				selectedPort = dataRegister;
				break;
			case 1:
				if(streams[selectedPort] != null) 
				{
					Stream str = streams[selectedPort] as Stream;
					str.WriteByte(dataRegister);
					this.statusRegister = STATUS_SUCCESS;
				}
				else
					this.statusRegister = STATUS_ERR;
				break;
			case 2:
				if(streams[selectedPort] != null) 
				{
					Stream str = streams[selectedPort] as Stream;
					int data = str.ReadByte();
					if(data == -1)
						this.statusRegister = STATUS_EMPTY;
					else
					{
						this.statusRegister = STATUS_SUCCESS;
						this.dataRegister = (byte)data;
					}
				}
				else
					this.statusRegister = STATUS_ERR;
				break;
			}
		}
	}
}

