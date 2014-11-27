using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public class TokenFloatingPoint : AbstractToken
    {
        public readonly double Value;
        public TokenFloatingPoint(double val, int l)
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
