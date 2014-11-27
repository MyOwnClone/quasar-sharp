using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GruntXProductions.Quasar.VM.Debugger
{
    interface IPacket
    {
        void Send(Stream s);
        void Recieve(Stream s);
    }
}
