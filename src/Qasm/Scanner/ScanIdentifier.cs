using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public partial class Lexer
    {
        private AbstractToken scanIdentifier()
        {
            StringBuilder builder = new StringBuilder();
            do
            {
                builder.Append((char)readChar());
            } while (char.IsLetterOrDigit((char)peekChar()) || (char)peekChar() == '_');

            return new TokenIdentifier(builder.ToString(), lineNumber);
        }
    }
}
