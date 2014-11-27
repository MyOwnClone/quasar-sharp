using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar
{
    public enum AddressingMode
    {
        NONE = 0,
        IMMEDIATE = 1,
        IMMEDIATE_DOUBLE = 2,
        PC_RELATIVE = 3,
        REGISTER_DIRECT = 4,
        REGISTER_INDIRECT = 5,
        REGISTER_INDIRECT_WORD = 6,
        REGISTER_INDIRECT_BYTE = 7,
        CONDITION_CODE = 8,
    }
}
