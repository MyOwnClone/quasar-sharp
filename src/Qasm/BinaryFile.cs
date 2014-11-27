using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GruntXProductions.Quasar;

namespace GruntXProductions.Quasar.Assembler
{
    public class BinaryFile : QuasarExecutable
    {
        private List<IProgramData> instructions = new List<IProgramData>();
        private Dictionary<string, int> symbolTable = new Dictionary<string, int>();
        private int pos = 0;

        public override void Emit(IProgramData data)
        {
            instructions.Add(data);
            pos += data.GetLength();
        }

        public override void Emit(QuasarSymbol sym)
        {
            symbolTable[sym.Name] = pos;
        }

        public override void FinalizeExecutable()
        {
            int position = 0;
			uint org = 0;
            foreach (IProgramData data in instructions)
            {
                if (data is QuasarInstruction)
                {
                    QuasarInstruction ins = data as QuasarInstruction;
                    if (ins.Operand1 != null && ins.Operand1 is SymbolReferenceOperand)
                    {
                        SymbolReferenceOperand sref = ins.Operand1 as SymbolReferenceOperand;
                        if (sref.Relative)
                            sref.Address = (uint)(symbolTable[sref.Name] - position - ins.GetLength());
                        else
                            sref.Address = org + (uint)symbolTable[sref.Name];
                    }
                    else if (ins.Operand2 != null && ins.Operand2 is SymbolReferenceOperand)
                    {
                        SymbolReferenceOperand sref = ins.Operand2 as SymbolReferenceOperand;
                        if(sref.Relative)
                            sref.Address = (uint)(symbolTable[sref.Name] - position - ins.GetLength());
                        else
                            sref.Address = org + (uint)symbolTable[sref.Name];
                    }
                }
				else if (data is OrgDirective)
				{
					OrgDirective orgd = data as OrgDirective;
					org = orgd.Origin;
				}
                position += data.GetLength();
            }
			
        }

        public override void Generate(Stream output)
        {
            foreach (IProgramData ins in instructions)
                ins.WriteData(output);
        }
    }
}
