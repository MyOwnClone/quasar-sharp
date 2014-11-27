using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GruntXProductions.Quasar
{
    public interface IProgramData
    {
        int GetLength();
        void WriteData(Stream strm);
    }
}
