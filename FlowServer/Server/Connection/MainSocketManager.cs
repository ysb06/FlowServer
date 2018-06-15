using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace FlowServer.Server.Connection
{
    class MainSocketManager
    {
        public MainSocketManager()
        {

        }

        public void StartConnection()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 57783);

            ConsoleController.Log("ip " + localEndPoint.ToString());

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10);

            listener.BeginAccept(new AsyncCallback(AcceptCallBack), listener);
            ConsoleController.Log("Accepting...");
        }

        public void AcceptCallBack(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            Socket clientSocket = listener.EndAccept(ar);

            ConsoleController.Log("Connected : " + clientSocket.LocalEndPoint.ToString());
        }
    }
}
