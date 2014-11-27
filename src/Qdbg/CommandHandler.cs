using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Debugger
{
    public abstract class CommandHandler
    {
        public abstract string[] Names
        {
            get;
        }

        public abstract void Execute(string[] args, DebugClient client);
    }
}
