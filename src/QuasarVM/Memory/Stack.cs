using System;

namespace GruntXProductions.Quasar.VM
{
	public class Stack
	{
		private QuasarRam memory;
		private Emulator host;
		public Stack (Emulator emu)
		{
			this.memory = emu.Memory;
			this.host = emu;
		}
		
		public void PushInt32(uint i)
		{
			uint sp = host.GetGeneralPurposeRegister(Register.R14);
			memory.WriteInt32(sp, i);
			host.SetGeneralPurposeRegister(Register.R14, sp - 4);
		}
		
		public uint PopInt32()
		{
			uint sp = host.GetGeneralPurposeRegister(Register.R14) + 4;
			uint ret = memory.ReadInt32(sp);
			host.SetGeneralPurposeRegister(Register.R14, sp);
			return ret;
		}
	}
}

