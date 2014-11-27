using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class ReserveAssembler : AbstractAssembler
    {

        public ReserveAssembler()
            : base(new string[] { "reserve" })
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 1)
                cgen.CreateError("Directive {0} does not take {1} operands!", ins.Name, ins.Operands.Count);
            else if (!(ins.Operands[0] is TokenIntLiteral))
                cgen.CreateError("Integer size expected!");
            else
                str.Emit(new QuasarData(new byte[((TokenIntLiteral)ins.Operands[0]).Value]));
        }
    }
}
