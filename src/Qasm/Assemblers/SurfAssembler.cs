using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class SurfAssembler : AbstractAssembler
    {

        public SurfAssembler()
            : base(new string[] { "surf" })
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 1)
            {
                cgen.CreateError("surf does not take {0} arguments!", ins.Operands.Count);
            }
            else if (!(ins.Operands[0] is TokenRegister))
            {
                cgen.CreateError("Unsupported addressing mode for instruction '{0}'", ins.Name);
            }
            else
            {
                TokenRegister dest = ins.Operands[0] as TokenRegister;
                str.Emit(new QuasarInstruction(Opcode.SURF, new RegisterOperand(dest.Register)));
                    
            }
        }
    }
}
