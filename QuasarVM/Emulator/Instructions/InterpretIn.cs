using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
	public partial class Emulator
	{
		private void interpretIn(Instruction ins)
		{
			if(ins.Operand1.OperandAddressingMode == AddressingMode.DIRECT_REGISTER
			   && (ins.Operand2.OperandAddressingMode == AddressingMode.DIRECT_REGISTER
			    || ins.Operand2.OperandAddressingMode == AddressingMode.IMMEDIATE_32))
			{
				Register dest = (Register)ins.Operand1.Value;
				int port = ins.Operand2.OperandAddressingMode == AddressingMode.IMMEDIATE_32 ?
					(int)(uint)ins.Operand2.Value : (int)GetGeneralPurposeRegister((Register)ins.Operand2.Value);
				uint val = this.peripheralController.In(port);
				SetGeneralPurposeRegister(dest, val);
			}
			else
				throw new InvalidOpcodeException(ins);
		}
		
		
	}
	
}