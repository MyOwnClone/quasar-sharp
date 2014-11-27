using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
    public partial class Emulator
    {
        private void interpretMov(Instruction ins)
        {
            switch (ins.Operand1.OperandAddressingMode)
            {
                case AddressingMode.DIRECT_REGISTER:
                    interpretMovDirect(ins);
                    break;
                case AddressingMode.INDIRECT_REG8:
                case AddressingMode.INDIRECT_REG32:
                    interpretMovIndirect(ins);
                    break;
                default:
                    throw new InvalidOpcodeException(ins);
            }
        }

        private void interpretMovDirect(Instruction ins)
        {
            switch (ins.Operand2.OperandAddressingMode)
            {
                case AddressingMode.DIRECT_REGISTER:
                    interpretMovDirectDirect(ins);
                    break;
                case AddressingMode.IMMEDIATE_32:
                    interpretMovDirectImmediate(ins);
                    break;
                case AddressingMode.INDIRECT_REG32:
                    interpretMovDirectIndirect(ins);
                    break;
            }
        }

        private void interpretMovIndirect(Instruction ins)
        {
            switch (ins.Operand2.OperandAddressingMode)
            {
                case AddressingMode.DIRECT_REGISTER:
                    if (ins.Operand1.OperandAddressingMode == AddressingMode.INDIRECT_REG32)
                        interpretMovIndirectOffsetDirect32(ins);
                    else if (ins.Operand1.OperandAddressingMode == AddressingMode.INDIRECT_REG8)
                        interpretMovIndirectOffsetDirect8(ins);
                    break;

            }
        }


        private void interpretMovDirectDirect(Instruction ins)
        {
            Register reg1 = (Register)ins.Operand1.Value;
            Register reg2 = (Register)ins.Operand2.Value;
            this.SetGeneralPurposeRegister(reg1, GetGeneralPurposeRegister(reg2));
        }

        private void interpretMovDirectIndirect(Instruction ins)
        {
            IndirectOffset offs = (IndirectOffset)ins.Operand2.Value;
            Register reg1 = (Register)ins.Operand1.Value;
            Register reg2 = offs.Register;
            uint val = this.memory.ReadInt32((uint)((int)GetGeneralPurposeRegister(reg2) + offs.Offset));
            SetGeneralPurposeRegister(reg1, val);
        }

        private void interpretMovDirectImmediate(Instruction ins)
        {
            Register reg1 = (Register)ins.Operand1.Value;
            uint op2 = (uint)ins.Operand2.Value;
            this.SetGeneralPurposeRegister(reg1, op2);
        }


        private void interpretMovIndirectOffsetDirect32(Instruction ins)
        {
            IndirectOffset offs = (IndirectOffset)ins.Operand1.Value;
            Register reg1 = offs.Register;
            Register reg2 = (Register)ins.Operand2.Value;
            memory.WriteInt32((uint)((int)offs.Offset + (int)GetGeneralPurposeRegister(reg1)), GetGeneralPurposeRegister(reg2));
        }

        private void interpretMovIndirectOffsetDirect8(Instruction ins)
        {
            IndirectOffset offs = (IndirectOffset)ins.Operand1.Value;
            Register reg1 = offs.Register;
            Register reg2 = (Register)ins.Operand2.Value;
            memory.WriteInt8((uint)((int)GetGeneralPurposeRegister(reg1) + offs.Offset), (byte)GetGeneralPurposeRegister(reg2));
        }
    }

}