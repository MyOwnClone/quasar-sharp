using System;

namespace GruntXProductions.Quasar.VM
{
	public class GeneralProtectionException : Exception
	{
		private Opcode opcode;
		
		public Opcode Opcode
		{
			get
			{
				return this.opcode;
			}
		}
		
		public GeneralProtectionException (Opcode opcode)
		{
			this.opcode = opcode;	
		}
	}
}

