using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class WaitAssembler : AbstractAssembler
    {

        public WaitAssembler()
            : base(new string[] { "wait" })
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 0)
            {
                cgen.CreateError("{0} does not take {1} arguments!", ins.Name, ins.Operands.Count);
            }
            else
            {
                str.Emit(new QuasarInstruction(Opcode.WAIT));
            }
        }
    }
}
