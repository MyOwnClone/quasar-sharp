using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class DataAssembler : AbstractAssembler
    {

        public DataAssembler()
            : base(new string[] { "data", ".data"})
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter bw = new BinaryWriter(ms);
                foreach (AbstractToken tok in ins.Operands)
                {
                    if (tok is TokenStringLiteral)
                        bw.Write(Encoding.ASCII.GetBytes(((TokenStringLiteral)tok).Value));
                    else if (tok is TokenIntLiteral)
                        bw.Write((byte)((TokenIntLiteral)tok).Value);
                }

                str.Emit(new QuasarData(ms.ToArray()));
            }
        }
    }
}
