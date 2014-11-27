using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GruntXProductions.Quasar
{
    public abstract class QuasarExecutable : BytecodeStream
    {
        public virtual void FinalizeExecutable() { }
        public abstract void Generate(Stream output);
    }
}
