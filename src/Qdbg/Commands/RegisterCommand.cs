using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GruntXProductions.Quasar.Debugger.Commands
{
    public class RegisterCommand : CommandHandler
    {
        public override string[] Names
        {
            get
            {
                return new string[]{
                    "r",
                    "l"
                };
            }
        }

        public override void Execute(string[] args, DebugClient client)
        {
            if (!client.IsRunning)
            {
                client.SendPacket(new PacketRequest(DebugRequest.REGISTERS, 0, 0, 0, 0, new byte[] { }));
                PacketEvent e = client.WaitForEvent();
                using (BinaryReader br = new BinaryReader(new MemoryStream(e.Data)))
                {
                    while(br.BaseStream.Position < br.BaseStream.Length)
                        Console.WriteLine("{0}  =   0x{1:X8}", br.ReadString().PadRight(4), br.ReadUInt32());
                }
            }
        }



    }
}
