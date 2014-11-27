using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public class TokenIdentifier : AbstractToken
    {
        public string Value;
        public TokenIdentifier(string name, int l)
            : base(l)
        {
            this.Value = name;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
