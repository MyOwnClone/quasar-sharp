using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Debugger
{
    public enum DebugRequest
    {
        BREAK = 0,
        STEP = 1,
        READ = 2,
        WRITE = 3,
        REGISTERS = 4,
    }
}
