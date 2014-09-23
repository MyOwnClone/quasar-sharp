using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
	public partial class Emulator
	{
		private void interpretAdd(Instruction ins)
		{
			if(ins.Operand1.OperandAddressingMode == AddressingMode.DIRECT_REGISTER)
			{
				uint flags = GetGeneralPurposeRegister(Register.R12);
				Register dest = (Register)ins.Operand1.Value;
				uint op1 = GetGeneralPurposeRegister(dest);
				uint op2 = 0;
				if(ins.Operand2.OperandAddressingMode == AddressingMode.IMMEDIATE_32)
				{
					op2 = (uint)ins.Operand2.Value;
				}
				else if (ins.Operand2.OperandAddressingMode == AddressingMode.DIRECT_REGISTER)
				{
					op2 = GetGeneralPurposeRegister((Register)ins.Operand2.Value);
				}
				else
				{
					throw new InvalidOpcodeException(ins);
				}
				checked
				{
					try
					{
						uint res = op1 + op2;
						
						flags &= ~Flags.CARRY;
						flags &= ~Flags.OVERFLOW;
						
						if((res & 4294967296) != 0)
							flags |= Flags.SIGN;
						else
							flags &= ~Flags.SIGN;
						if(res == 0)
							flags |= Flags.ZERO;
						else
							flags &= ~Flags.ZERO;
						SetGeneralPurposeRegister(dest, res);
					}
					catch(OverflowException)
					{
						flags |= Flags.OVERFLOW;
						Console.WriteLine(op1);
						Console.WriteLine(op2);
					}
				}
				SetGeneralPurposeRegister(Register.R12, flags);
				
			}
			else
				throw new InvalidOpcodeException(ins);
		}
		
		
	}
	
}