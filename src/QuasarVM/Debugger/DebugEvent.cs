using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.VM.Debugger
{
    public enum DebugEvent
    {
        BREAK = 0,
        STEP_NEXT = 1,
        EXCEPTION = 2,
        RCV_DATA = 3,
        ERROR = 4,
    }
}
