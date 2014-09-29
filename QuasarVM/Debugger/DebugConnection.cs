using System;
using System.IO;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace GruntXProductions.Quasar.VM
{
	public class DebugConnection
	{
		private TextWriter textWriter;
		private TextReader textReader;
		private Emulator host;
		private Stream stream;
		private List<uint> breakPoints = new List<uint>();
		
		public DebugConnection (Emulator host, Stream stream)
		{
			textWriter = new StreamWriter(stream);
			textReader = new StreamReader(stream);
			this.stream = stream;
			this.host = host;
		}
		
		public void Listen()
		{
			new Thread(() => 
			{
				while(true)
				{
					string[] args = textReader.ReadLine().Split(' ');
					string command = args[0];
					switch(command.ToUpper())
					{
					case "INFO":
						infoRequest(args);
						break;
					case "MEMORY":
						memoryRequest(args);
						break;
					}
					stream.Flush();
				}
			}).Start();
		}
		
		public void Update (Emulator emu)
		{
			uint pc = emu.GetGeneralPurposeRegister(Register.R15);
			if(breakPoints.Contains(pc))
			{
				
			}
		}
		
		private void infoRequest(string[] args)
		{
			switch(args[1].ToUpper())
			{
			case "REGISTERS":
				sendRegisters();
				break;
			}
		}
		
		private void memoryRequest(string[] args)
		{
			uint start = uint.Parse(args[1], System.Globalization.NumberStyles.HexNumber);
			uint end = uint.Parse(args[2], System.Globalization.NumberStyles.HexNumber);
			sendMemory(start, end);
		}
		
		private void sendRegisters()
		{
			StringBuilder sb = new StringBuilder("");
			sb.Append("REGISTERS ");
			for(int i = 0; i < 16; i++)
			{
				sb.AppendFormat("r{0}={1};", i.ToString(), host.GetGeneralPurposeRegister((Register)i));
			}
			textWriter.WriteLine(sb.ToString());
			textWriter.Flush();
		}
		
		private void sendMemory(uint start, uint end)
		{
			StringBuilder sb = new StringBuilder("");
			sb.AppendFormat("REGION {0} ", start.ToString("x8"));
			for(uint p = start; p < end; p++)
				sb.Append(this.host.Memory[p].ToString("x2"));
			textWriter.WriteLine(sb.ToString());
			textWriter.Flush();
		}
	}
}

