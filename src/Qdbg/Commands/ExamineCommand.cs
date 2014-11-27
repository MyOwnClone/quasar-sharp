using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GruntXProductions.Quasar.Debugger.Commands
{
    public class ExamineCommand : CommandHandler
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
                uint start = uint.Parse(args[1], System.Globalization.NumberStyles.AllowHexSpecifier);
                uint end = uint.Parse(args[1], System.Globalization.NumberStyles.AllowHexSpecifier);
                client.SendPacket(new PacketRequest(DebugRequest.READ, start, end, 0, 0, new byte[] { }));
                PacketEvent e = client.WaitForEvent();
                using (BinaryReader br = new BinaryReader(new MemoryStream(e.Data)))
                {
                }
            }
        }

    }
}
