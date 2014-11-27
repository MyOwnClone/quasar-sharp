using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public class TokenRegister : AbstractToken
    {
        public QuasarRegister Register;
        public TokenRegister(QuasarRegister reg, int i)
            : base(i)
        {
            this.Register = reg;
        }
    }
}
