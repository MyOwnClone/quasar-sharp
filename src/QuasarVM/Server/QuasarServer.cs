using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace GruntXProductions.Quasar.VM.Server
{
    public class QuasarServer : Device
    {
        private TcpListener tcpListener;
        private Thread listener;
        private List<QuasarConnection> connections = new List<QuasarConnection>();

        public QuasarServer()
        {
            this.tcpListener = new TcpListener(IPAddress.Loopback, 3216);
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
                
                QuasarConnection connection = new QuasarConnection(client);
                connection.Listen();
                this.connections.Add(connection);
            }
        }

        public override void Init(Emulator emu)
        {
        }

      
    }
}
