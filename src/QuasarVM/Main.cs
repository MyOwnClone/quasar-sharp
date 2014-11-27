using System;
using System.Threading;
using System.IO;
using GruntXProductions.Quasar.VM.Server;
using GruntXProductions.Quasar.VM.Debugger;

namespace GruntXProductions.Quasar.VM
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args)
		{
			string input = null;
			uint ramsize = 0x20000000;
			bool debug = false;
			for(int i = 0; i < args.Length; i++)
			{
				if(!args[i].StartsWith("-") && input == null)
					input = args[i];
				else if (args[i] == "-S")
				{
					debug = true;
				}
				else if (args[i] == "-m")
				{
					string amount = args[++i];
					uint rs;
					if(uint.TryParse(amount, out rs)) 
					{
						ramsize = rs * 0x100000;
					}
					else
						Fail("Size {0} is not valid!", amount);
				}
			}
			
			if(input == null)
				Fail("No input specified!");

            
            Emulator emu = new Emulator(new QuasarRam(ramsize));
            byte[] data = File.ReadAllBytes(input);
            for (uint i = 0; i < data.Length; i++)
                emu.Memory[i] = data[i];
            if (debug)
            {
                DebugServer serv = new DebugServer(emu);
                serv.Start();
                serv.WaitForConnection();
            }

            emu.Emulate();


		}
		
		private static void Fail(string format, params object[] args)
		{
			Console.Error.WriteLine(format, args);
			Environment.Exit(-1);
		}
	}
}
