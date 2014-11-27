using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar;
using GruntXProductions.Phosphorus;

namespace GruntXProductions.Quasar.Debugger.Commands
{
    public class StepCommand : CommandHandler
    {
        public override string[] Names
        {
            get
            {
                return new string[]{
                    "step"
                };
            }
        }

        public override void Execute(string[] args, DebugClient client)
        {
            if (!client.IsRunning)
            {
                client.SendPacket(new PacketRequest(DebugRequest.STEP, 0, 0, 0, 0, new byte[] { }));
                PacketEvent e = client.WaitForEvent();
                Instruction ins = Instruction.Fetch(e.Argument0, e.Data);
                Console.WriteLine("Step 0x{0:X8}\t{1}", e.Argument0, ins.ToString());
            }
        }
    }
}
