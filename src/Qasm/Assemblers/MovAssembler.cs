using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class MovAssembler : AbstractAssembler
    {

        public MovAssembler() : base(new string[]{"mov", "movb", "movw"})
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 2)
                cgen.CreateError("Instruction {0} does not take {1} operand(s)!", ins.Name, ins.Operands.Count);
            else if (ins.Name == "movb")
            {
                if (ins.Operands[0] is TokenIndirectRegister &&
                    ins.Operands[1] is TokenRegister)
                {
                    TokenIndirectRegister r1 = ins.Operands[0] as TokenIndirectRegister;
                    TokenRegister r2 = ins.Operands[1] as TokenRegister;
                    str.Emit(new QuasarInstruction(Opcode.MOV, new IndirectRegisterOffsetOperand(r1.Register, AddressingMode.REGISTER_INDIRECT_BYTE, r1.Offset),
                        new RegisterOperand(r2.Register)));
                }
                else if (ins.Operands[0] is TokenIndirectRegister &&
                        ins.Operands[1] is TokenRegister)
                {
                    TokenIndirectRegister r1 = ins.Operands[0] as TokenIndirectRegister;
                    TokenRegister r2 = ins.Operands[1] as TokenRegister;
                    str.Emit(new QuasarInstruction(Opcode.MOV, new IndirectRegisterOffsetOperand(r1.Register, AddressingMode.REGISTER_INDIRECT_BYTE, r1.Offset),
                        new RegisterOperand(r2.Register)));
                }
            }
			else if (ins.Name == "movw")
            {
                if (ins.Operands[0] is TokenIndirectRegister &&
                    ins.Operands[1] is TokenRegister)
                {
                    TokenIndirectRegister r1 = ins.Operands[0] as TokenIndirectRegister;
                    TokenRegister r2 = ins.Operands[1] as TokenRegister;
                    str.Emit(new QuasarInstruction(Opcode.MOV, new IndirectRegisterOffsetOperand(r1.Register, AddressingMode.REGISTER_INDIRECT_WORD, r1.Offset),
                        new RegisterOperand(r2.Register)));
                }
            }
            else
            {
                if (ins.Operands[0] is TokenRegister &&
                    ins.Operands[1] is TokenRegister)
                {
                    TokenRegister r1 = ins.Operands[0] as TokenRegister;
                    TokenRegister r2 = ins.Operands[1] as TokenRegister;
                    str.Emit(new QuasarInstruction(Opcode.MOV, new RegisterOperand(r1.Register),
                        new RegisterOperand(r2.Register)));
                }
                else if (ins.Operands[0] is TokenRegister &&
                    ins.Operands[1] is TokenIntLiteral)
                {
                    TokenRegister r1 = ins.Operands[0] as TokenRegister;
                    TokenIntLiteral i1 = ins.Operands[1] as TokenIntLiteral;
                    str.Emit(new QuasarInstruction(Opcode.MOV, new RegisterOperand(r1.Register),
                        new IntegerOperand((int)i1.Value)));
                }
                else if (ins.Operands[0] is TokenRegister &&
                    ins.Operands[1] is TokenIdentifier)
                {
                    TokenRegister r1 = ins.Operands[0] as TokenRegister;
                    TokenIdentifier i1 = ins.Operands[1] as TokenIdentifier;
                    str.Emit(new QuasarInstruction(Opcode.MOV, new RegisterOperand(r1.Register),
                        new SymbolReferenceOperand(i1.Value)));
                }
                else if (ins.Operands[0] is TokenRegister &&
                    ins.Operands[1] is TokenIndirectRegister)
                {
                    TokenRegister r1 = ins.Operands[0] as TokenRegister;
                    TokenIndirectRegister r2 = ins.Operands[1] as TokenIndirectRegister;
                    str.Emit(new QuasarInstruction(Opcode.MOV, new RegisterOperand(r1.Register),
                        new IndirectRegisterOffsetOperand(r2.Register, r2.Offset)));
                }
				else if (ins.Operands[0] is TokenIndirectRegister &&
                    ins.Operands[1] is TokenRegister)
                {
                    TokenIndirectRegister r1 = ins.Operands[0] as TokenIndirectRegister;
                  
                    TokenRegister r2 = ins.Operands[1] as TokenRegister;
                    str.Emit(new QuasarInstruction(Opcode.MOV, new IndirectRegisterOffsetOperand(r1.Register, AddressingMode.REGISTER_INDIRECT, r1.Offset),
                        new RegisterOperand(r2.Register)));
                }
                else
                {
                    cgen.CreateError("Unsupported addressing mode for instruction {0}", ins.Name);
                }
            }
        }
    }
}
