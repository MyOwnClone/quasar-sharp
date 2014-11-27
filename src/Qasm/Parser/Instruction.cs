using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;

namespace GruntXProductions.Quasar.Assembler.Parser
{
    public class Instruction
    {
        private string name;
        private List<AbstractToken> operands;
        private int line;

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public IList<AbstractToken> Operands
        {
            get
            {
                return this.operands;
            }
        }

        public int Line
        {
            get
            {
                return this.line;
            }
        }

        public Instruction(int line, string name, List<AbstractToken> operands)
        {
            this.line = line;
            this.name = name;
            this.operands = operands;
        }
    }
}
