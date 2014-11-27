using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public partial class Lexer
    {
        private AbstractToken scanStringLiteral()
        {
            readChar();
            StringBuilder accum = new StringBuilder();
            while ((char)peekChar() != '\"' && peekChar() != -1)
            {
                accum.Append((char)readChar());
            }
            readChar();
            return new TokenStringLiteral(accum.ToString(), lineNumber);
        }
    }
}
