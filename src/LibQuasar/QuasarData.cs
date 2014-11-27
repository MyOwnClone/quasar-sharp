using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GruntXProductions.Quasar
{
    public class QuasarData : IProgramData
    {
        private byte[] data;

        public QuasarData(byte[] data)
        {
            this.data = data;
        }

        public void WriteData(Stream str)
        {
            str.Write(data, 0, data.Length);
        }

        public int GetLength()
        {
            return data.Length;
        }
    }
}
