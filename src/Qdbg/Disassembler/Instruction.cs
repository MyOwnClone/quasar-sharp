using System;
using System.IO;
using System.Text;

namespace GruntXProductions.Phosphorus
{
    public class Instruction
    {
        private Opcode opcode;
        private Operand operand1;
        private Operand operand2;
        private int size;
        private bool hasOperands = false;
        private uint address;

        public Opcode OperationCode
        {
            get
            {
                return this.opcode;
            }
        }

        public Operand Operand1
        {
            get
            {
                return operand1;
            }
        }

        public Operand Operand2
        {
            get
            {
                return operand2;
            }
        }

        public int Size
        {
            get
            {
                return this.size;
            }
        }

        public bool HasOperands
        {
            get
            {
                return hasOperands;
            }
        }

        public uint Address
        {
            get
            {
                return this.address;
            }
        }

        public Instruction(Opcode opcode)
        {
            this.opcode = opcode;
            this.hasOperands = false;
            this.size = 1;
        }

        public Instruction(Opcode opcode, int size, uint address, AddressingMode am, object op1)
        {
            this.opcode = opcode;
            this.operand1.Value = op1;
            this.operand1.OperandAddressingMode = am;
            this.hasOperands = true;
            this.size = size;
            this.address = address;
        }

        public Instruction(Opcode opcode, int size, uint address, AddressingMode am, object op1, AddressingMode am2, object op2)
        {
            this.opcode = opcode;
            this.operand1.Value = op1;
            this.operand1.OperandAddressingMode = am;
            this.operand2.Value = op2;
            this.operand2.OperandAddressingMode = am2;
            this.hasOperands = true;
            this.size = size;
            this.address = address;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.opcode.ToString() + "");
            if (size != 1)
            {
                if (operand1.OperandAddressingMode == AddressingMode.IMMEDIATE_32)
                    sb.AppendFormat("\t0x{0}", ((uint)this.operand1.Value).ToString("x8"));
                else if (operand1.OperandAddressingMode == AddressingMode.DIRECT_REGISTER)
                    sb.AppendFormat("\t%{0}", operand1.Value.ToString());
                else if (operand1.Value is IndirectOffset)
                {
                    IndirectOffset offset = operand1.Value as IndirectOffset;
                    if (offset.Offset == 0)
                        sb.AppendFormat("\t@{0}", offset.Register.ToString());
                    else
                        sb.AppendFormat("\t@{0}:0x{1:x}", offset.Register.ToString(), offset.Offset);
                }
                if (operand2.OperandAddressingMode != AddressingMode.NONE)
                {
                    if (operand1.OperandAddressingMode != AddressingMode.CONDITION_CODE)
                        sb.Append(",\t");
                    else
                        sb.Append(new string[] { "", " S", " Z", " C" }[(byte)operand1.Value]);
                    if (operand2.OperandAddressingMode == AddressingMode.IMMEDIATE_32)
                        sb.AppendFormat("\t0x{0}", ((uint)this.operand2.Value).ToString("x8"));
                    else if (operand2.OperandAddressingMode == AddressingMode.DIRECT_REGISTER)
                        sb.AppendFormat("\t%{0}", operand2.Value.ToString());
                    else if (operand2.Value is IndirectOffset)
                    {
                        IndirectOffset offset = operand2.Value as IndirectOffset;
                        if (offset.Offset == 0)
                            sb.AppendFormat("\t@{0}", offset.Register.ToString());
                        else
                            sb.AppendFormat("\t@{0}:0x{1:x}", offset.Register.ToString(), offset.Offset);
                    }
                }
            }
            return sb.ToString();
        }

        public string MachineCode()
        {
            try
            {
                if (this.size == 1 || this.operand1.OperandAddressingMode == AddressingMode.NONE)
                    return ((byte)this.opcode).ToString("x2");
                else
                {
                    StringBuilder sb = new StringBuilder();
                    byte am1 = (byte)this.operand1.OperandAddressingMode;
                    byte am2 = (byte)this.operand2.OperandAddressingMode;
                    byte descr = (byte)(am1 | (am2 << 4));
                    sb.Append(((byte)this.opcode).ToString("x2") + " ");
                    sb.Append(descr.ToString("x2") + " ");
                    foreach (byte b in operand1.GetBytes())
                        sb.Append(b.ToString("x2") + " ");
                    foreach (byte b in operand2.GetBytes())
                        sb.Append(b.ToString("x2") + " ");

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                return "???";
            }
        }

        public static Instruction Fetch(uint org_add, byte[] code)
        {
            using (MemoryStream str = new MemoryStream(code))
            {
                BinaryReader br = new BinaryReader(str);
                byte original = br.ReadByte();
                Opcode op = (Opcode)(original & 0x7f);
                AddressingMode am1 = AddressingMode.NONE;
                AddressingMode am2 = AddressingMode.NONE;
                bool hasOperands = ((int)original & 128) == 0;

                if (hasOperands)
                {
                    byte descr = br.ReadByte();
                    am1 = (AddressingMode)(descr & 0x0F);
                    am2 = (AddressingMode)((descr & 0xF0) >> 4);
                    object op1 = ReadOperand(br, am1);
                    object op2 = ReadOperand(br, am2);
                    int size = (int)(str.Position - org_add);
                    return new Instruction(op, size, org_add, am1, op1, am2, op2);

                }
                else
                    return new Instruction(op, 1, org_add, AddressingMode.NONE, null);
            }

        }

        private static object ReadOperand(BinaryReader br, AddressingMode am)
        {
            switch (am)
            {
                case AddressingMode.DIRECT_REGISTER:
                    return (Register)br.ReadByte();
                case AddressingMode.CONDITION_CODE:
                    return (byte)br.ReadByte();
                case AddressingMode.INDIRECT_REG32:
                case AddressingMode.INDIRECT_REG16:
                case AddressingMode.INDIRECT_REG8:
                    return new IndirectOffset((Register)br.ReadByte(), br.ReadInt16());
                case AddressingMode.IMMEDIATE_32:
                    return br.ReadUInt32();
            }
            return null;
        }


    }
}

