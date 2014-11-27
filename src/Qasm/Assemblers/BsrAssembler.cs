using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class BsrAssembler : AbstractAssembler
    {

        public BsrAssembler()
            : base(new string[] { "bsr"})
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 1)
                cgen.CreateError("Instruction {0} does not take {1} operand(s)!", ins.Name, ins.Operands.Count);
            else if(ins.Operands[0] is TokenIdentifier)
            {
                TokenIdentifier label = ins.Operands[0] as TokenIdentifier;
                
                str.Emit(new QuasarInstruction(Opcode.BSR, new SymbolReferenceOperand(label.Value, true)));
            }
        }
    }
}
