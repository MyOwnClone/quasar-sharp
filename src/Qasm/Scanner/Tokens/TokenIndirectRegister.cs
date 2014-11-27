using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public class TokenIndirectRegister : AbstractToken
    {
        public QuasarRegister Register;
        public int Offset;
        public TokenIndirectRegister(QuasarRegister reg, int offset, int i)
            : base(i)
        {
            this.Offset = offset;
            this.Register = reg;
        }

        public TokenIndirectRegister(QuasarRegister reg, int i)
            : base(i)
        {
            this.Offset = 0;
            this.Register = reg;
        }
    }
}
