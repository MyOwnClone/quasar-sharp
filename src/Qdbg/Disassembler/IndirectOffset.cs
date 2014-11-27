using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Phosphorus
{
    public class IndirectOffset
    {
        private Register register;
        private short offset;

        public Register Register
        {
            get
            {
                return this.register;
            }
        }

        public short Offset
        {
            get
            {
                return this.offset;
            }
        }

        public IndirectOffset(Register reg, short offset)
        {
            this.register = reg;
            this.offset = offset;
        }
    }
}
