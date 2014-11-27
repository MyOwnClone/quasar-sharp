using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GruntXProductions.Quasar
{
    public class QuasarInstruction : IProgramData
    {
        private Opcode opcode;
        private QuasarOperand operand1 = null;
        private QuasarOperand operand2 = null;

        public Opcode OperationCode
        {
            get
            {
                return opcode;
            }
        }

        public QuasarOperand Operand1
        {
            get
            {
                return operand1;
            }
        }

        public QuasarOperand Operand2
        {
            get
            {
                return operand2;
            }
        }

        public QuasarInstruction(Opcode op)
        {
            this.opcode = op;
        }

        public QuasarInstruction(Opcode op, QuasarOperand operand)
        {
            this.opcode = op;
            this.operand1 = operand;
        }

        public QuasarInstruction(Opcode op, QuasarOperand operand1, QuasarOperand operand2)
        {
            this.opcode = op;
            this.operand1 = operand1;
            this.operand2 = operand2;
        }

        public void WriteData(Stream ostream)
        {
            BinaryWriter bw = new BinaryWriter(ostream);
            {
                byte op = (byte)(((byte)opcode) | (operand1 == null ? 0x80 : 0));
                bw.Write(op);
                if(operand1 != null)
                {
                    bw.Write(getOperandDesc());
                    bw.Write(operand1.ToByteArray());
                    if (operand2 != null)
                        bw.Write(operand2.ToByteArray());
                }
            }
        }

        public int GetLength()
        {
            if (operand1 == null)
                return 1;
            else if (operand2 == null)
                return 2 + operand1.ToByteArray().Length;
            else
                return 2 + operand1.ToByteArray().Length + operand2.ToByteArray().Length;
        }

        private byte getOperandDesc()
        {
            byte am1 = operand1 == null ? (byte)AddressingMode.NONE : (byte)operand1.GetAddressingMode();
            byte am2 = operand2 == null ? (byte)AddressingMode.NONE : (byte)operand2.GetAddressingMode();
			
            return (byte)(am1 | (am2 << 4));
        }
    }
}
