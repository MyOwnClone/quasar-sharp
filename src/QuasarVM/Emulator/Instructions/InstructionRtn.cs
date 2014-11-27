using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
    public partial class Emulator
    {
        private void interpretRtn(Instruction ins)
        {
            if (!ins.HasOperands)
            {
                uint pc = stack.PopInt32();
                SetGeneralPurposeRegister(Register.R15, (uint)pc);
            }
            else
                throw new InvalidOpcodeException(ins);
        }


    }

}