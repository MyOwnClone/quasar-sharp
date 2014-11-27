using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class RtnAssembler : AbstractAssembler
    {

        public RtnAssembler()
            : base(new string[] { "rtn" })
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
                str.Emit(new QuasarInstruction(Opcode.RTN));
            }
        }
    }
    public class IRtnAssembler : AbstractAssembler
    {

        public IRtnAssembler()
            : base(new string[] { "irtn" })
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
                str.Emit(new QuasarInstruction(Opcode.IRTN));
            }
        }
    }
}
