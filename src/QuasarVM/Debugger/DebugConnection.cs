using System;
using System.IO;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace GruntXProductions.Quasar.VM.Debugger
{
	public class DebugConnection 
	{
		private TextWriter textWriter;
		private TextReader textReader;
		private Emulator host;
		private Stream stream;
		private List<uint> breakPoints = new List<uint>();
		
		public DebugConnection (Emulator host, Stream stream)
		{
			textWriter = new StreamWriter(stream);
			textReader = new StreamReader(stream);
			this.stream = stream;
			this.host = host;
            this.breakPoints.Add(0x0000);
		}
		
		public void Listen()
		{
			new Thread(() => 
			{
				while(true)
				{
                    PacketRequest req = new PacketRequest();
                    req.Recieve(this.stream);

                    switch (req.Request)
                    {
                        case DebugRequest.BREAK:
                            this.host.Halt();
                            sendPacket(new PacketEvent(DebugEvent.BREAK, this.host.GetGeneralPurposeRegister(Register.R15), 0,
                                0, 0, new byte[] { }));
                            break;
                        case DebugRequest.REGISTERS:
                            sendRegisters();
                            break;
                        case DebugRequest.STEP:
                            this.host.Step();
                            uint pc = host.GetGeneralPurposeRegister(Register.R15);
                            sendPacket(new PacketEvent(DebugEvent.STEP_NEXT, pc, 0,
                                0, 12, new byte[] { host.Memory[pc], host.Memory[pc + 1], host.Memory[pc + 2], host.Memory[pc + 3],
                                host.Memory[pc + 4], host.Memory[pc + 5], host.Memory[pc + 6], host.Memory[pc + 7], host.Memory[pc + 8], 
                                host.Memory[pc + 9], host.Memory[pc + 10], host.Memory[pc + 11] }));
                            break;
                    }

				}
			}).Start();
		}
		
		public void Update (Emulator emu)
		{
			uint pc = emu.GetGeneralPurposeRegister(Register.R15);
			if(breakPoints.Contains(pc))
			{
                this.host.Halt();
                sendPacket(new PacketEvent(DebugEvent.BREAK, pc, 0,
                    0, 4, new byte[] { host.Memory[pc], host.Memory[pc + 1], host.Memory[pc + 2], host.Memory[pc + 3] }));
			}
		}
		
		private void sendRegisters()
		{
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                for(int i = 0; i < 15; i++)
                {
                    bw.Write(String.Format("r{0}", i));
                    bw.Write(host.GetGeneralPurposeRegister((Register)i));
                }
            }
            byte[] bytes = ms.ToArray();
            sendPacket(new PacketEvent(DebugEvent.RCV_DATA, 2, 0, 0, (uint)bytes.Length, bytes));
		}
		
		private void sendMemory(uint start, uint end)
		{
            byte[] dat = new byte[end - start];
            for(uint i = 0; i < dat.Length; i++)
                dat[i] = host.Memory[start + i];
            PacketEvent evn = new PacketEvent(DebugEvent.RCV_DATA, 0x01, 0, 0, end - start, dat);
		}

        private void sendPacket(IPacket packet)
        {
            packet.Send(this.stream);
            this.stream.Flush();
        }
	}
}

