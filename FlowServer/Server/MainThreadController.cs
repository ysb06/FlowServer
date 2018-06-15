using FlowServer.Server.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlowServer.Server
{
    class MainThreadController
    {
        private Thread threadMain;

        private MainSocketManager mainSocket;


        public MainThreadController()
        {
            ConsoleController.Broadcast += ConsoleMessage_Receive;

            threadMain = new Thread(Run);
            threadMain.Start();
        }

        private void Run()
        {
            ConsoleController.Debug("Initializing main server thread is complete.", "Server Thread");

            mainSocket = new MainSocketManager();
            mainSocket.StartConnection();
        }

        private void ConsoleMessage_Receive(string message)
        {
            Console.WriteLine(message);
        }
    }
}
