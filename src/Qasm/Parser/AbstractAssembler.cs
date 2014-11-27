using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar;

namespace GruntXProductions.Quasar.Assembler.Parser
{
    public abstract class AbstractAssembler
    {
        private string[] mnemonics;

        public string[] Mnemonics
        {
            get
            {
                return mnemonics;
            }
        }

        public AbstractAssembler(string[] mnemonics)
        {
            this.mnemonics = mnemonics;
        }

        public abstract void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str);
    }
}
