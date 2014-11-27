using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using GruntXProductions.Quasar.Debugger.Commands;

namespace GruntXProductions.Quasar.Debugger
{
    class Program
    {
        static void Main(string[] args)
        {
            DebugClient client = new DebugClient(IPAddress.Loopback);
            client.RegisterCommand(new BreakCommand());
            client.RegisterCommand(new RegisterCommand());
            client.RegisterCommand(new StepCommand());
            client.StartShell();
        }

    }
}
