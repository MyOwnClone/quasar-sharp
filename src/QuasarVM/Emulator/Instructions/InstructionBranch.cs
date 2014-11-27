using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
	public partial class Emulator
	{
		private void interpretBranch(Instruction ins)
		{
			if(ins.Operand1.OperandAddressingMode == AddressingMode.CONDITION_CODE
			   && ins.Operand2.OperandAddressingMode == AddressingMode.IMMEDIATE_32)
			{
				uint condition = (byte)ins.Operand1.Value;
				uint flags = GetGeneralPurposeRegister(Register.R12);
				int jump = ((int)(uint)ins.Operand2.Value);
				int pc = (int)GetGeneralPurposeRegister(Register.R15);
				pc += jump;
				bool canJump = false;
				switch(condition)
				{
				case ConditionCode.ALWAYS:
					canJump = true;
					break;
				case ConditionCode.SIGN:
					canJump = (flags & Flags.SIGN) != 0;
					break;
				case ConditionCode.ZERO:
					canJump = (flags & Flags.ZERO) != 0;
					break;
				case ConditionCode.CARRY:
					canJump = (flags & Flags.CARRY) != 0;
					break;
				case ConditionCode.OVERFLOW:
					canJump = (flags & Flags.OVERFLOW) != 0;
					break;
				}
				if(canJump)
					SetGeneralPurposeRegister(Register.R15, (uint)pc);
			}
			else
				throw new InvalidOpcodeException(ins);
		}
		
		
	}
	
}