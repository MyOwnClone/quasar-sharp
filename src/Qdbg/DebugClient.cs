using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace GruntXProductions.Quasar.Debugger
{
    public class DebugClient
    {
        private TcpClient client = new TcpClient();
        private NetworkStream stream;
        private bool reading = false;
        private bool sending = false;
        private bool running = true;
        private bool eventRecieved = false;
        private PacketEvent lastEvent;
        private List<CommandHandler> commands = new List<CommandHandler>();

        public bool IsRunning
        {
            get
            {
                return running;
            }
        }

        public DebugClient(IPAddress addr)
        {
            client.Connect(addr, 6969);
            stream = client.GetStream();
        }

        public void StartShell()
        {
            listen();
            while (true)
            {
                Console.Write("(qdbg) ");
                string input = Console.ReadLine();
                string[] args = input.Split(' ');
                interpretCommand(args);
            }
        }

        public void RegisterCommand(CommandHandler handler)
        {
            this.commands.Add(handler);
        }

        private void interpretCommand(string[] args)
        {
            if (args.Length >= 1)
            {
                foreach (CommandHandler handler in this.commands)
                {
                    if (handler.Names.Contains<string>(args[0]))
                    {
                        handler.Execute(args, this);
                        return;
                    }
                }
            }
        }

        public void SendPacket(IPacket packet)
        {
            while (reading) ;
            sending = true;
            packet.Send(stream);
            stream.Flush();
            sending = false;
        }

        public PacketEvent WaitForEvent()
        {
            eventRecieved = false;
            while (!eventRecieved) Thread.Sleep(1);
            eventRecieved = true;
            return lastEvent;
        }

        private void listen()
        {
            new Thread(() =>
            {
                while (true)
                {
                    while (!stream.DataAvailable || sending) Thread.Sleep(1);
                    reading = true;
                    PacketEvent evn = new PacketEvent();
                    evn.Recieve(stream);
                    reading = false;
                    eventRecieved = true;
                    lastEvent = evn;
                    this.handleEvent(evn);
                }
            }).Start();
        }

        private void handleEvent(PacketEvent e)
        {
            switch (e.Event)
            {
                case DebugEvent.BREAK:
                    this.running = false;
                    break;
                case DebugEvent.STEP_NEXT:
                    this.running = false;
                    break;
            }
        }
    }
}
