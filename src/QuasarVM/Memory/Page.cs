using System;

namespace GruntXProductions.Quasar.VM
{
	public static class Page
	{
		public const byte EXECUTE = 1;
		public const byte READ = 2;
		public const byte WRITE = 4;
		public const byte PRESENT = 8;
	}
}

