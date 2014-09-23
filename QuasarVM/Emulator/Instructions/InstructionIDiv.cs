using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
	public partial class Emulator
	{
		private void interpretIDiv(Instruction ins)
		{
			if(ins.Operand1.OperandAddressingMode == AddressingMode.DIRECT_REGISTER)
			{
				uint flags = GetGeneralPurposeRegister(Register.R12);
				Register dest = (Register)ins.Operand1.Value;
				int op1 = (int)GetGeneralPurposeRegister(dest);
				int op2 = 0;
				if(ins.Operand2.OperandAddressingMode == AddressingMode.IMMEDIATE_32)
				{
					op2 = (int)(uint)ins.Operand2.Value;
				}
				else if (ins.Operand2.OperandAddressingMode == AddressingMode.DIRECT_REGISTER)
				{
					op2 = (int)GetGeneralPurposeRegister((Register)ins.Operand2.Value);
				}
				else
				{
					throw new InvalidOpcodeException(ins);
				}
				checked
				{
					try
					{
						int res = op1 / op2;
						
						flags &= ~Flags.CARRY;
						flags &= ~Flags.OVERFLOW;
						
						if(res < 0)
							flags |= Flags.SIGN;
						else
							flags &= ~Flags.SIGN;
						if(res == 0)
							flags |= Flags.ZERO;
						else
							flags &= ~Flags.ZERO;
						SetGeneralPurposeRegister(dest, (uint)res);
					}
					catch(OverflowException)
					{
						flags |= Flags.OVERFLOW;
					}
				}
				SetGeneralPurposeRegister(Register.R12, flags);
				
			}
			else
				throw new InvalidOpcodeException(ins);
		}
		
		
	}
	
}