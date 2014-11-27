using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public partial class Lexer
    {
        private AbstractToken scanIntLiteral()
        {
            if ((char)peekChar(0) == '0' && (char)peekChar(1) == 'x')
                return scanHexNumber();
            else
            {
                StringBuilder accum = new StringBuilder();
                do
                {
                    accum.Append((char)readChar());
                    if (((char)peekChar()) == '.')
                        return scanFloatingPoint(accum);
                } while (char.IsDigit((char)peekChar()));
                return new TokenIntLiteral(Int64.Parse(accum.ToString()), lineNumber);
            }
        }

        private AbstractToken scanFloatingPoint(StringBuilder accum)
        {
            do
            {
                accum.Append((char)readChar());
            } while (char.IsDigit((char)peekChar()));
            return new TokenFloatingPoint(Double.Parse(accum.ToString()), lineNumber);
        }

        private AbstractToken scanHexNumber()
        {
            readChar();
            readChar();
            StringBuilder accum = new StringBuilder();
            do
            {
                accum.Append((char)readChar());
            } while (char.IsDigit((char)peekChar()) || "abcdefABCDEF".Contains((char)peekChar()));
            return new TokenIntLiteral(Int64.Parse(accum.ToString(), System.Globalization.NumberStyles.HexNumber), lineNumber);
        }
    }
}
