using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
	public partial class Emulator
	{
		private void interpretOut(Instruction ins)
		{
			if((ins.Operand1.OperandAddressingMode == AddressingMode.IMMEDIATE_32 |
			    ins.Operand1.OperandAddressingMode == AddressingMode.DIRECT_REGISTER) 
			   && ins.Operand2.OperandAddressingMode == AddressingMode.DIRECT_REGISTER)
			{
				Register src = (Register)ins.Operand2.Value;
				int port = ins.Operand1.OperandAddressingMode == AddressingMode.IMMEDIATE_32 ?
					(int)(uint)ins.Operand1.Value : (int)GetGeneralPurposeRegister((Register)ins.Operand2.Value);
				this.peripheralController.Out(port, GetGeneralPurposeRegister(src));
			}
			else
				throw new InvalidOpcodeException(ins);
		}
		
		
	}
	
}