using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
	public partial class Emulator
	{
		private void interpretLctl(Instruction ins)
		{
			if(ins.Operand1.OperandAddressingMode == AddressingMode.DIRECT_REGISTER
			   && ins.Operand2.OperandAddressingMode == AddressingMode.NONE)
			{
				this.controlRegister = GetGeneralPurposeRegister((Register)ins.Operand1.Value);
				if((this.controlRegister & 0x2) != 0)
					memory.EnablePaging(this.pageDirectory);
				else
					memory.DisablePaging();
			}
			else
				throw new InvalidOpcodeException(ins);
		}
		
		
	}
	
}