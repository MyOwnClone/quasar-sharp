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
			
			QuasarRam memory = new QuasarRam(ramsize);
			Emulator emu = new Emulator(memory);
			if(debug)
			{
				DebugServer dbg = new DebugServer(emu);
				dbg.Start();
			}
			byte[] program = File.ReadAllBytes(input);
			memory.Write(0, program.Length, 0, program);
			DeviceSerialController serialController = new DeviceSerialController();
			serialController.OpenComPort(0, Console.OpenStandardInput());
			serialController.OpenComPort(1, Console.OpenStandardOutput());
			emu.RegisterDevice(serialController);
			emu.RegisterDevice(new DeviceROMController(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("GruntXProductions.Quasar.VM.ROM.boot.bin")));
			
			ScreenForm form = new ScreenForm();
	
			emu.RegisterDevice(new DeviceTextScreen(form));
			new Thread(() => {
				emu.Emulate();
			}).Start();
			
			System.Windows.Forms.Application.Run(form);
		}
		
		private static void Fail(string format, params object[] args)
		{
			Console.Error.WriteLine(format, args);
			Environment.Exit(-1);
		}
	}
}
