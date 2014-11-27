using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class ShrAssembler : AbstractAssembler
    {

        public ShrAssembler()
            : base(new string[] { "shr" })
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 2)
            {
                cgen.CreateError("{0} does not take {1} arguments!", ins.Name, ins.Operands.Count);
            }
            else if (!(ins.Operands[0] is TokenRegister) || (!(ins.Operands[1] is TokenRegister) && !(ins.Operands[1] is TokenIntLiteral) && !(ins.Operands[1] is TokenIdentifier)))
            {
                cgen.CreateError("Unsupported addressing mode for instruction '{0}'", ins.Name);
            }
            else
            {
                TokenRegister op1 = ins.Operands[0] as TokenRegister;
                Opcode opcode = Opcode.SHR;
                if (ins.Operands[1] is TokenRegister)
                {
                    TokenRegister op2 = ins.Operands[1] as TokenRegister;
                    str.Emit(new QuasarInstruction(opcode, new RegisterOperand(op1.Register), new RegisterOperand(op2.Register)));
                }
                else if (ins.Operands[1] is TokenIntLiteral)
                {
                    TokenIntLiteral op2 = ins.Operands[1] as TokenIntLiteral;
                    str.Emit(new QuasarInstruction(opcode, new RegisterOperand(op1.Register), new IntegerOperand((int)op2.Value)));
                }
                else if (ins.Operands[1] is TokenIdentifier)
                {
                    TokenIdentifier ident = ins.Operands[1] as TokenIdentifier;
                    str.Emit(new QuasarInstruction(opcode, new RegisterOperand(op1.Register), new SymbolReferenceOperand(ident.Value)));
                }

            }
        }
    }
    public class SarAssembler : AbstractAssembler
    {

        public SarAssembler()
            : base(new string[] { "sar" })
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 2)
            {
                cgen.CreateError("{0} does not take {1} arguments!", ins.Name, ins.Operands.Count);
            }
            else if (!(ins.Operands[0] is TokenRegister) || (!(ins.Operands[1] is TokenRegister) && !(ins.Operands[1] is TokenIntLiteral) && !(ins.Operands[1] is TokenIdentifier)))
            {
                cgen.CreateError("Unsupported addressing mode for instruction '{0}'", ins.Name);
            }
            else
            {
                TokenRegister op1 = ins.Operands[0] as TokenRegister;
                Opcode opcode = Opcode.SAR;
                if (ins.Operands[1] is TokenRegister)
                {
                    TokenRegister op2 = ins.Operands[1] as TokenRegister;
                    str.Emit(new QuasarInstruction(opcode, new RegisterOperand(op1.Register), new RegisterOperand(op2.Register)));
                }
                else if (ins.Operands[1] is TokenIntLiteral)
                {
                    TokenIntLiteral op2 = ins.Operands[1] as TokenIntLiteral;
                    str.Emit(new QuasarInstruction(opcode, new RegisterOperand(op1.Register), new IntegerOperand((int)op2.Value)));
                }
                else if (ins.Operands[1] is TokenIdentifier)
                {
                    TokenIdentifier ident = ins.Operands[1] as TokenIdentifier;
                    str.Emit(new QuasarInstruction(opcode, new RegisterOperand(op1.Register), new SymbolReferenceOperand(ident.Value)));
                }

            }
        }
    }
}
