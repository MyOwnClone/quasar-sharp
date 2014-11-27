using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class NotAssembler : AbstractAssembler
    {

        public NotAssembler()
            : base(new string[] { "not" })
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 1)
            {
                cgen.CreateError("{0} does not take {1} arguments!", ins.Name, ins.Operands.Count);
            }
            else if (!(ins.Operands[0] is TokenRegister))
            {
                cgen.CreateError("Unsupported addressing mode for instruction '{0}'", ins.Name);
            }
            else
            {
                TokenRegister op1 = ins.Operands[0] as TokenRegister;
                str.Emit(new QuasarInstruction(Opcode.NOT, new RegisterOperand(op1.Register)));
            }
        }
    }
}
