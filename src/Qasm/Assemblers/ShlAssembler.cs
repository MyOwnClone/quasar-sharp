using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class ShlAssembler : AbstractAssembler
    {

        public ShlAssembler()
            : base(new string[] { "shl" })
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
                Opcode opcode = Opcode.SHL;
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
    public class SalAssembler : AbstractAssembler
    {

        public SalAssembler()
            : base(new string[] { "sal" })
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
                Opcode opcode = Opcode.SAL;
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
