using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler.Assemblers
{
    public class WordsAssembler : AbstractAssembler
    {

        public WordsAssembler()
            : base(new string[] { "word", "words" })
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter bw = new BinaryWriter(ms);
                foreach (AbstractToken tok in ins.Operands)
                {
                    if (tok is TokenIntLiteral)
                        bw.Write((int)((TokenIntLiteral)tok).Value);
                }

                str.Emit(new QuasarData(ms.ToArray()));
            }
        }
    }
}
