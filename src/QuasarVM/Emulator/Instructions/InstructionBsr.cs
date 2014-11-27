using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
    public partial class Emulator
    {
        private void interpretBsr(Instruction ins)
        {
            if (ins.Operand1.OperandAddressingMode == AddressingMode.IMMEDIATE_32
            && ins.Operand2.OperandAddressingMode == AddressingMode.NONE)
            {
                int jump = (int)(uint)ins.Operand1.Value;
                uint pc = GetGeneralPurposeRegister(Register.R15);
                uint sp = GetGeneralPurposeRegister(Register.R14);
                SetGeneralPurposeRegister(Register.R15, (uint)(pc + jump));

                memory.WriteInt32(sp, pc);

                this.SetGeneralPurposeRegister(Register.R14, sp - 4);

            }
            else
                throw new InvalidOpcodeException(ins);
        }


    }

}