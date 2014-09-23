using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
	public partial class Emulator
	{
		private void interpretIRtn(Instruction ins)
		{
			if(ins.Operand1.OperandAddressingMode == AddressingMode.NONE
			   && ins.Operand2.OperandAddressingMode == AddressingMode.NONE)
			{
				for(int i = 15; i >= 0; i--)
					SetGeneralPurposeRegister((Register)i, stack.PopInt32());
				this.controlRegister = stack.PopInt32();
				this.doubleFault = false;
				this.cpuException = false;
			}
			else
				throw new InvalidOpcodeException(ins);
		}
		
		
	}
	
}