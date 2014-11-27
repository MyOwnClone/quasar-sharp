using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.Assembler.Parser
{
    public class CodeGenerator
    {
        private static List<AbstractAssembler> assemblers = new List<AbstractAssembler>();
        private QuasarExecutable output;

        public CodeGenerator(QuasarExecutable output) 
        {
            this.output = output;
        }

        public void Assemble(IList<object> instructions)
        {
            foreach (object o in instructions)
            {
                if (o is Instruction)
                {
                    Instruction ins = o as Instruction;
					if(!assemblers.Any(a => a.Mnemonics.Contains<string>(ins.Name)))
						CreateError("Unknown instruction {0}", ins.Name);
					else
					{
	                    AbstractAssembler asm = assemblers.First(a => a.Mnemonics.Contains<string>(ins.Name));
	                    if (asm != null)
	                    {
	                        asm.Assemble(this, ins, this.output);
	                    }
					}
                }
                else if (o is Label)
                {
                    Label lbl = o as Label;
                    output.Emit(new QuasarSymbol(lbl.Name));
                }
            }
        }

        public static void RegisterAssembler(AbstractAssembler asm)
        {
            assemblers.Add(asm);
        }

        public void CreateError(string format, params object[] args)
        {
            Console.Error.WriteLine(format, args);
        }
    }
}
