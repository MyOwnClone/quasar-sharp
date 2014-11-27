using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace GruntXProductions.Quasar.VM.Debugger
{
    public class DebugServer : Device
    {
        private TcpListener tcpListener;
        private Emulator host;
        private Thread listener;
        private List<DebugConnection> connections = new List<DebugConnection>();

        public DebugServer(Emulator emu)
        {
            this.host = emu;
            this.host.RegisterDevice(this);
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

        public void WaitForConnection()
        {
            while (this.connections.Count == 0) Thread.Sleep(1);
        }

        public override void Init(Emulator emu)
        {
        }

        public override void Update(Emulator emu)
        {
            foreach (DebugConnection conn in this.connections)
                conn.Update(emu);
        }
    }
}

