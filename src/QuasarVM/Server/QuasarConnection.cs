using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net.Sockets;
namespace GruntXProductions.Quasar.VM.Server
{
    public class QuasarConnection : Device
    {
        private Emulator emu;
        private BinaryWriter bw;
        private NetworkStream netStream;
        private TcpClient client;
        private Thread emulatorThread;

        public QuasarConnection (TcpClient client)
		{
            this.netStream = client.GetStream();
			this.bw = new BinaryWriter(this.netStream);
            this.client = client;
		}
		
		public void Listen()
		{
			new Thread(() =>
            {
                byte[] data = new byte[16];
				while(true)
				{
                    netStream.Read(data, 0, 16);
                    executeCommand(data[0], data);
                    if (client.Available == 0)
                        wait();
                    
				}
			}).Start();
		}

        private void executeCommand(byte command, byte[] packet)
        {
            uint arg0 = BitConverter.ToUInt32(packet, 1);
            switch (command)
            {
                case 0:
                    this.emu = new Emulator(new QuasarRam(arg0));
                    this.emu.RegisterDevice(this);
                    this.emu.RegisterDevice(new DeviceROMController(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("GruntXProductions.Quasar.VM.ROM.boot.bin")));
                    break;
                case 1:
                    this.emu.Reset();
                    break;
                case 2:
                    if (emulatorThread == null)
                    {
                        emulatorThread = new Thread(this.emu.Emulate);
                        emulatorThread.Start();
                    }
                    break;
                case 3:
                    this.emu.PeripheralController.RequestIOPort(this, (int)arg0);
                    break;
                case 4:
                    this.emu.Power(arg0 != 0);
                    break;
            }
        }

        private void wait()
        {
            this.netStream.Flush();
            while (this.client.Available == 0) Thread.Sleep(1);
        }

        public override void Init(Emulator emu)
        {
        }

        public override void RecieveData(int port, uint data)
        {
            Console.WriteLine("sending data from port {0}!", port.ToString("x4"));
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter tmp = new BinaryWriter(ms);
                tmp.Write((byte)0);
                tmp.Write((uint)port);
                tmp.Write((uint)data);
                while (tmp.BaseStream.Length < 16)
                    tmp.Write((byte)0);
                this.bw.Write(ms.ToArray());
                netStream.Flush();
            }
        }

        
    }
}
