using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
    public partial class Emulator
    {
        private void interpretWait(Instruction ins)
        {
            if (!ins.HasOperands)
            {
                this.Halt();
            }
            else
                throw new InvalidOpcodeException(ins);
        }


    }

}