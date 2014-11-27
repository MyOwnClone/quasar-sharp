using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Phosphorus
{
    public struct Operand
    {
        public object Value;
        public AddressingMode OperandAddressingMode;

        public byte[] GetBytes()
        {
            if (OperandAddressingMode == AddressingMode.INDIRECT_REG32 ||
                (OperandAddressingMode == AddressingMode.INDIRECT_REG16 ||
                (OperandAddressingMode == AddressingMode.INDIRECT_REG8)))
            {
                IndirectOffset off = Value as IndirectOffset;
                return new byte[]{(byte)off.Register, (byte)(off.Offset & 0xFF), (byte)((off.Offset &
                    0xFF00) >> 8)};
            }
            else if (OperandAddressingMode == AddressingMode.IMMEDIATE_32)
                return BitConverter.GetBytes((uint)Value);
            else if (OperandAddressingMode == AddressingMode.DIRECT_REGISTER)
                return BitConverter.GetBytes((byte)(Register)Value);
            else if (OperandAddressingMode == AddressingMode.CONDITION_CODE)
                return new byte[] { (byte)Value };
            else
                return new byte[] { };
        }
    }
}
