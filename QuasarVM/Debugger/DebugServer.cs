using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace GruntXProductions.Quasar.VM
{
	public class DebugServer
	{
		private TcpListener tcpListener;
		private Emulator host;
		private Thread listener;
		private List<DebugConnection> connections = new List<DebugConnection>();
		
		public DebugServer (Emulator emu)
		{
			this.host = emu;
			this.tcpListener = new TcpListener(IPAddress.Loopback, 6969);
		}
		
		public void Start()
		{
			this.listener = new Thread(listen);
			this.listener.Start();
		}
		
		private void listen()
		{
  			this.tcpListener.Start();
			while (true)
			{
				
				TcpClient client = this.tcpListener.AcceptTcpClient();
	
				DebugConnection connection = new DebugConnection(this.host, client.GetStream());
				connection.Listen();
				this.connections.Add(connection);
			}
		}
		
		
	}
}

