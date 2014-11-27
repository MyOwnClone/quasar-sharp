using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public abstract class AbstractToken
    {
        public readonly int Line;

        public AbstractToken(int line)
        {
            this.Line = line;
        }
    }
}
