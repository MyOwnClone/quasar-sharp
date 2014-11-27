using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class SwpAssembler : AbstractAssembler
    {

        public SwpAssembler()
            : base(new string[] { "swp" })
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 2)
            {
                cgen.CreateError("{0} does not take {1} arguments!", ins.Name, ins.Operands.Count);
            }
            else if (!(ins.Operands[0] is TokenRegister && ins.Operands[1] is TokenRegister))
            {
                cgen.CreateError("Unsupported addressing mode for instruction '{0}'", ins.Name);
            }
            else
            {
                TokenRegister op1 = ins.Operands[0] as TokenRegister;
                TokenRegister op2 = ins.Operands[1] as TokenRegister;
                str.Emit(new QuasarInstruction(Opcode.SWP, new RegisterOperand(op1.Register), new RegisterOperand(op2.Register)));

            }
        }
    }
}
