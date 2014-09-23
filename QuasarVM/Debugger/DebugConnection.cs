using System;
using System.IO;
using System.Threading;
using System.Text;

namespace GruntXProductions.Quasar.VM
{
	public class DebugConnection
	{
		private TextWriter textWriter;
		private TextReader textReader;
		private Emulator host;
		private Stream stream;
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
					}
					stream.Flush();
				}
			}).Start();
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
	}
}

