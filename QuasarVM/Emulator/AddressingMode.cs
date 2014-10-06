using System;

namespace GruntXProductions.Quasar.VM
{
	public enum AddressingMode
	{
		NONE = 0,
		IMMEDIATE_32 = 1,
		IMMEDIATE_64 = 2,
		PC_DIRECT = 3,
		DIRECT_REGISTER = 4,
		INDIRECT_REG32 = 5,
		INDIRECT_REG16 = 6,
		INDIRECT_REG8 = 7,
        INDIRECT_REGOFFSET32 = 8,
        INDIRECT_REGOFFSET16 = 9,
        INDIRECT_REGOFFSET8 = 10,
	}
}

