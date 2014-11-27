using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class OutAssembler : AbstractAssembler
    {

        public OutAssembler()
            : base(new string[] { "out" })
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 2)
            {
                cgen.CreateError("{0} does not take {1} arguments!", ins.Name, ins.Operands.Count);
            }
            else if (!(ins.Operands[0] is TokenIntLiteral || ins.Operands[0] is TokenRegister) && !(ins.Operands[1] is TokenRegister))
            {
                cgen.CreateError("Unsupported addressing mode for instruction '{0}'", ins.Name);
            }
            else
            {
                TokenRegister op2 = ins.Operands[1] as TokenRegister;
                Opcode opcode = Opcode.OUT;
                if (ins.Operands[0] is TokenRegister)
                {
                    TokenRegister op1 = ins.Operands[0] as TokenRegister;
                    str.Emit(new QuasarInstruction(opcode, new RegisterOperand(op1.Register), new RegisterOperand(op2.Register)));
                }
                else if (ins.Operands[0] is TokenIntLiteral)
                {
                    TokenIntLiteral op1 = ins.Operands[0] as TokenIntLiteral;
                    str.Emit(new QuasarInstruction(opcode, new IntegerOperand((int)op1.Value), new RegisterOperand(op2.Register)));
                }
                

            }
        }
    }
}
