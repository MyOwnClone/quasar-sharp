using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Debugger.Commands
{
    public class BreakCommand : CommandHandler
    {
        public override string[] Names
        {
            get
            {
                return new string[]{
                    "b",
                    "break"
                };
            }
        }

        public override void Execute(string[] args, DebugClient client)
        {
            if (client.IsRunning)
            {
                client.SendPacket(new PacketRequest(DebugRequest.BREAK, 0, 0, 0, 0, new byte[] { }));
                client.WaitForEvent();
            }
        }
    }
}
