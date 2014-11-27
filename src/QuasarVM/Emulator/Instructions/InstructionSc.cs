using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
    public partial class Emulator
    {
        private void interpretSc(Instruction ins)
        {
            if (!ins.HasOperands)
            {
                this.Interrupt(0x00);
            }
            else
                throw new InvalidOpcodeException(ins);
        }


    }

}