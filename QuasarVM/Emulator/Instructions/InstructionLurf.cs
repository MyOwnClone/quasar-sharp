using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
	public partial class Emulator
	{
		private void interpretLurf(Instruction ins)
		{
			if(ins.Operand1.OperandAddressingMode == AddressingMode.DIRECT_REGISTER
			   && ins.Operand2.OperandAddressingMode == AddressingMode.NONE)
			{
				this.userRegisterFileBase = GetGeneralPurposeRegister((Register)ins.Operand1.Value);
			}
			else
				throw new InvalidOpcodeException(ins);
		}
		
		
	}
	
}