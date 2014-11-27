using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Parser
{
    public class Label
    {
        private string name;

        public string Name
        {
            get
            {
                return this.name;
            }
			set
			{
				this.name = value;
			}
        }

        public Label(string name)
        {
            this.name = name;
        }
    }
}
