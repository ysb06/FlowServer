using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlowServer.Server.Core
{
    class MainThreadController
    {
        private Thread threadMain;

        private Socket socket;
        private readonly IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 50321);

        public MainThreadController()
        {
            ConsoleController.Broadcast += ConsoleMessage_Receive;

            threadMain = new Thread(Run);
            threadMain.Start();
        }

        private void Run()
        {
            ConsoleController.Debug("Initializing main server thread is complete.", "Server Thread");

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            socket.Bind(ipPoint);
            socket.Listen(1);
            socket.BeginAccept(SocketAcceptCallback, new object());
        }

        private void ConsoleMessage_Receive(string message)
        {
            Console.WriteLine(message);
        }

        private void SocketAcceptCallback(IAsyncResult iar)
        {
            ConsoleController.Debug("Accept: " + socket.Connected);
        }
    }
}
