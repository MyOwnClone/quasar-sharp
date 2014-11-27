using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar
{
    public abstract class QuasarOperand
    {
        public abstract byte[] ToByteArray();
        public abstract AddressingMode GetAddressingMode();
    }

    public class IntegerOperand : QuasarOperand
    {
        private int integer;
        public IntegerOperand(int i)
        {
            this.integer = i;
        }

        public override byte[] ToByteArray()
        {
            return BitConverter.GetBytes(this.integer);
        }

        public override AddressingMode GetAddressingMode()
        {
            return AddressingMode.IMMEDIATE;
        }

        public static implicit operator IntegerOperand(int i)
        {
            return new IntegerOperand(i);
        }
    }

    public class SpFloatOperand : QuasarOperand
    {
        private float fvalue;
        public SpFloatOperand(float i)
        {
            this.fvalue = i;
        }

        public override byte[] ToByteArray()
        {
            return BitConverter.GetBytes(this.fvalue);
        }

        public override AddressingMode GetAddressingMode()
        {
            return AddressingMode.IMMEDIATE;
        }
    }

    public class DpFloatOperand : QuasarOperand
    {
        private double fvalue;
        public DpFloatOperand(double i)
        {
            this.fvalue = i;
        }

        public override byte[] ToByteArray()
        {
            return BitConverter.GetBytes(this.fvalue);
        }

        public override AddressingMode GetAddressingMode()
        {
            return AddressingMode.IMMEDIATE_DOUBLE;
        }
    }

    public class RegisterOperand : QuasarOperand
    {
        private int index;
        public RegisterOperand(int i)
        {
            this.index = i;
        }

        public RegisterOperand(QuasarRegister reg)
        {
            this.index = (int)reg;
        }

        public override byte[] ToByteArray()
        {
            return new byte[] { (byte)this.index };
        }

        public override AddressingMode GetAddressingMode()
        {
            return AddressingMode.REGISTER_DIRECT;
        }

        public static implicit operator RegisterOperand(int i)
        {
            return new RegisterOperand(i);
        }

        public static implicit operator RegisterOperand(QuasarRegister reg)
        {
            return new RegisterOperand(reg);
        }
    }


    public class IndirectRegisterOffsetOperand : QuasarOperand
    {
        private int offset;
        private int index;
        private AddressingMode addressingMode;
        public IndirectRegisterOffsetOperand(int i)
        {
            this.index = i;
            this.addressingMode = AddressingMode.REGISTER_INDIRECT;
        }

        public IndirectRegisterOffsetOperand(QuasarRegister reg, int offset)
        {
            this.index = (int)reg;
            this.addressingMode = AddressingMode.REGISTER_INDIRECT;
            this.offset = offset;
        }

        public IndirectRegisterOffsetOperand(QuasarRegister reg, AddressingMode am, int offset)
        {
            this.index = (int)reg;
            this.addressingMode = am;
            this.offset = offset;
        }

        public override byte[] ToByteArray()
        {
            return new byte[] { (byte)this.index, (byte)(this.offset & 0xFF), (byte)((this.offset & 0xFF00) >> 4) };
        }

        public override AddressingMode GetAddressingMode()
        {
            return addressingMode;
        }


    }
    public class ConditionCodeOperand : QuasarOperand
    {
        private int condition;
        public ConditionCodeOperand(QuasarConditionCode code)
        {
            this.condition = (int)code;
        }

        public override byte[] ToByteArray()
        {
            return new byte[]{(byte)condition};
        }

        public override AddressingMode GetAddressingMode()
        {
            return AddressingMode.CONDITION_CODE;
        }
    }

    public class SymbolReferenceOperand : QuasarOperand
    {
        public uint Address = 0;
        private string name;
        private bool relative = false;

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public bool Relative
        {
            get
            {
                return this.relative;
            }
        }

        public SymbolReferenceOperand(string name)
        {
            this.name = name;
        }

        public SymbolReferenceOperand(string name, bool rel)
        {
            this.name = name;
            this.relative = rel;
        }

        public override byte[] ToByteArray()
        {
            return BitConverter.GetBytes(this.Address);
        }

        public override AddressingMode GetAddressingMode()
        {
            return AddressingMode.IMMEDIATE;
        }


    }
}
