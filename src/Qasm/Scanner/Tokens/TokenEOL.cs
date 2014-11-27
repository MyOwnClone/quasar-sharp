using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public class TokenEOL : AbstractToken
    {
        public TokenEOL(int l)
            : base(l)
        {

        }
        public override string ToString()
        {
            return "End of Line";
        }
    }
}
