using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public class ScanningException : Exception
    {
        public readonly string Error;
        public ScanningException(string msg)
        {
            Console.WriteLine(msg);
            this.Error = msg;
        }
    }
}
