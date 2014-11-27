using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public class TokenHash : AbstractToken
    {
        public TokenHash(int l)
            : base(l)
        {

        }
        public override string ToString()
        {
            return "#";
        }
    }
}
