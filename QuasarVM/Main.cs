using System;
using System.Threading;
using System.IO;

namespace GruntXProductions.Quasar.VM
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args)
		{
			string input = null;
			uint ramsize = 0xFFFFFFFF;
			QuasarRam memory = new QuasarRam(ramsize);
			Emulator emu = new Emulator(memory);
			
			for(int i = 0; i < args.Length; i++)
			{
				if(!args[i].StartsWith("-") && input == null)
					input = args[i];
				else if (args[i] == "-S")
				{
					DebugServer dbg = new DebugServer(emu);
					dbg.Start();
				}
			}
			
			
			byte[] program = File.ReadAllBytes(input);
			memory.Write(0, program.Length, 0, program);

			emu.RegisterDevice(new DeviceSerial(0, Console.OpenStandardOutput()));
			
			ScreenForm form = new ScreenForm();
	
			emu.RegisterDevice(new DeviceTextScreen(form));
			new Thread(() => {
				emu.Emulate();
			}).Start();
			
			System.Windows.Forms.Application.Run(form);
		}
	}
}
