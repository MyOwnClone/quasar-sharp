using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
	public partial class Emulator
	{
		private void interpretSctl(Instruction ins)
		{
			if(ins.Operand1.OperandAddressingMode == AddressingMode.DIRECT_REGISTER
			   && ins.Operand2.OperandAddressingMode == AddressingMode.NONE)
			{
				SetGeneralPurposeRegister((Register)ins.Operand1.Value, controlRegister);
			}
			else
				throw new InvalidOpcodeException(ins);
		}
		
		
	}
	
}