using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public partial class Lexer
    {
        private AbstractToken scanRegister()
        {
            readChar();
            StringBuilder accum = new StringBuilder();
            while (char.IsLetterOrDigit((char)peekChar()) && peekChar() != -1)
            {
                accum.Append((char)readChar());
            }

            string reg = accum.ToString();
            int res = 0;
            if (!(reg.ToLower().StartsWith("r") || reg.ToLower().StartsWith("s") || reg.ToLower().StartsWith("d"))
                || !Int32.TryParse(reg.Substring(1), out res) || res > 15)
                throw new ScanningException(String.Format("{0} is not a register!", reg));

            if (reg.ToLower().StartsWith("s"))
                res += 16;
            else if (reg.ToLower().StartsWith("d"))
                res += 32;
            return new TokenRegister((QuasarRegister)res, this.lineNumber);
        }

        private AbstractToken scanIndirectRegister()
        {
            readChar();
            StringBuilder accum = new StringBuilder();
            while (char.IsLetterOrDigit((char)peekChar()) && peekChar() != -1)
            {
                accum.Append((char)readChar());
            }

            string reg = accum.ToString();
            int res = 0;
            if (!(reg.ToLower().StartsWith("r")) || !Int32.TryParse(reg.Substring(1), out res) || res > 15)
                throw new ScanningException(String.Format("{0} is not a register!", reg));
            if (peekChar() == ':')
            {
                readChar();
                TokenIntLiteral il = (TokenIntLiteral)scanIntLiteral();
                return new TokenIndirectRegister((QuasarRegister)res, (int)il.Value, this.lineNumber);
            }
            else
                return new TokenIndirectRegister((QuasarRegister)res, this.lineNumber);
        }
    }
}
