using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public class TokenColon : AbstractToken
    {
        public TokenColon(int l)
            : base(l)
        {

        }
        public override string ToString()
        {
            return ":";
        }
    }
}
