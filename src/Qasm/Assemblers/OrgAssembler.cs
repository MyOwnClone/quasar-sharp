using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;

namespace GruntXProductions.Quasar.Assembler
{
	public class OrgAssembler : AbstractAssembler
    {

        public OrgAssembler()
            : base(new string[] { "org" })
        {
        }

        public override void Assemble(CodeGenerator cgen, Instruction ins, BytecodeStream str)
        {
            if (ins.Operands.Count != 1)
            {
                cgen.CreateError("{0} does not take {1} arguments!", ins.Name, ins.Operands.Count);
            }
            else if (!(ins.Operands[0] is TokenIntLiteral))
            {
                cgen.CreateError("Unsupported addressing mode for instruction '{0}'", ins.Name);
            }
            else
            {
                TokenIntLiteral il = ins.Operands[0] as TokenIntLiteral;
				str.Emit(new OrgDirective((uint)il.Value));
            }
        }
	}
	
	public class OrgDirective : IProgramData
	{
		private uint org = 0;
		public uint Origin
		{
			get
			{
				return org;
			}
		}
		
		public OrgDirective(uint i)
		{
			org = i;
		}
		
        public int GetLength()
		{
			return 0;
		}
		
        public void WriteData(Stream strm)
		{
		}
	}
}

