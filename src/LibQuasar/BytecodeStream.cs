using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar
{
    public abstract class BytecodeStream
    {
        public abstract void Emit(IProgramData data);
        public abstract void Emit(QuasarSymbol sym);
    }
}
