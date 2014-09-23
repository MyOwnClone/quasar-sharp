using System;

namespace GruntXProductions.Quasar.VM
{
	public class InvalidOpcodeException : Exception
	{
		private Instruction instruction;
		
		public Instruction FaultingInstruction
		{
			get
			{
				return this.instruction;
			}
		}
		
		public InvalidOpcodeException (Instruction ins)
		{
			this.instruction = ins;
		}
	}
}

