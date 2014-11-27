using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class LfpAssembler : AbstractAssembler
    {

        public LfpAssembler()
            : base(new string[] { "lfp"})
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 2)
                cgen.CreateError("Instruction {0} does not take {1} operand(s)!", ins.Name, ins.Operands.Count);
           
            
            
        }
    }
}
