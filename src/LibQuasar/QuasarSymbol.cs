using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar
{
    public class QuasarSymbol
    {
        private string name;

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public QuasarSymbol(string name)
        {
            this.name = name;
        }
    }
}
