using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public class TokenIntLiteral : AbstractToken
    {
        public readonly long Value;
        public TokenIntLiteral(long val, int l)
            : base(l)
        {
            this.Value = val;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
