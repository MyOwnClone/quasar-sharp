using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class JmpAssembler : AbstractAssembler
    {

        public JmpAssembler()
            : base(new string[] { "br", "bs", "bv", "bc", "bz", "be", "bne", "ba", "bb", "bl", "bg", "ble", "bge"})
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 1)
                cgen.CreateError("Instruction {0} does not take {1} operand(s)!", ins.Name, ins.Operands.Count);
            else if(ins.Operands[0] is TokenIdentifier)
            {
                TokenIdentifier label = ins.Operands[0] as TokenIdentifier;
                QuasarConditionCode code = QuasarConditionCode.NONE;
                switch (ins.Name)
                {
                    case "bc":
                        code = QuasarConditionCode.C;
                        break;
                    case "bs":
                        code = QuasarConditionCode.N;
                        break;
                    case "bv":
                        code = QuasarConditionCode.V;
                        break;
                    case "bz":
                        code = QuasarConditionCode.Z;
                        break;
                    case "be":
                        code = QuasarConditionCode.EQ;
                        break;
                    case "bne":
                        code = QuasarConditionCode.NE;
                        break;
                    case "ba":
                        code = QuasarConditionCode.AB;
                        break;
                    case "bb":
                        code = QuasarConditionCode.BL;
                        break;
                    case "bl":
                        code = QuasarConditionCode.LT;
                        break;
                    case "bg":
                        code = QuasarConditionCode.GT;
                        break;
                    case "ble":
                        code = QuasarConditionCode.LE;
                        break;
                    case "bge":
                        code = QuasarConditionCode.GE;
                        break;
                }
                str.Emit(new QuasarInstruction(Opcode.BR, new ConditionCodeOperand(code), new SymbolReferenceOperand(label.Value, true)));
            }
        }
    }
}
