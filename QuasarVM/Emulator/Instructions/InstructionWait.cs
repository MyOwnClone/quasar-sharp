using System;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
	public partial class Emulator
	{
		private void interpretWait(Instruction ins)
		{
			if(!ins.HasOperands)
			{
				while(this.interruptController.IrqPending)
					Thread.Yield();
			}
			else
				throw new InvalidOpcodeException(ins);
		}
		
		
	}
	
}