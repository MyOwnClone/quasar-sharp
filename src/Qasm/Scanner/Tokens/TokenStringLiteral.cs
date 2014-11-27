using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public class TokenStringLiteral : AbstractToken
    {
        public readonly string Value;
        public TokenStringLiteral(string val, int l)
            : base(l)
        {
            this.Value = val;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
