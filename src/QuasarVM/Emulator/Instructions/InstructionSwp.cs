using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
    public partial class Emulator
    {
        private void interpretSwp(Instruction ins)
        {
            if (ins.Operand1.OperandAddressingMode == AddressingMode.DIRECT_REGISTER &&
            ins.Operand2.OperandAddressingMode == AddressingMode.DIRECT_REGISTER)
            {
                Register reg1 = (Register)ins.Operand1.Value;
                Register reg2 = (Register)ins.Operand2.Value;
                uint val = SetGeneralPurposeRegister(reg1, GetGeneralPurposeRegister(reg2));
                SetGeneralPurposeRegister(reg2, val);
            }
            else
                throw new InvalidOpcodeException(ins);
        }


    }

}