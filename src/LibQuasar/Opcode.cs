using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar
{
    public enum Opcode
    {
        NOP = 0,
        MOV = 1,
        SWP = 2,
        BR = 3,
        BSR = 4,
        RTN = 5,
        CMP = 6,
        TST = 0x07,
        SURF = 0x08,
        SSRF = 0x09,
        SPDR = 0x0A,
        SIVT = 0x0B,
        SCTL = 0x0C,
        LURF = 0x0D,
        LSRF = 0x0E,
        LPDR = 0x0F,
        LIVT = 0x10,
        LCTL = 0x11,
        ADD = 0x12,
        SUB = 0x13,
        DIV = 0x14,
        MUL = 0x15,
        IDIV = 0x17,
        IMUL = 0x16,
        MOD = 0x18,
        NEG = 0x19,
        NOT = 0x1A,
        BOR = 0x1B,
        AND = 0x1C,
        XOR = 0x1D,
        SHL = 0x1E,
        SHR = 0x1F,
        SAL = 0x20,
        SAR = 0x21,
        SC = 0x22,
        IN = 0x23,
        OUT = 0x24,
        WAIT = 0x25,
        IRTN = 0x26,
        LFP = 0x27
    }
}
