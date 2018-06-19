using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace FlowServer.Server.Connection
{
    public delegate void ClientHandler(FlowClient client);
    /// <summary>
    /// 클라이언트 접속 관리. 클라이언트 요청 수락 및 클라이언트 객체 관리
    /// </summary>
    class MainSocketManager
    {
        private readonly List<FlowClient> clients = new List<FlowClient>();

        public MainSocketManager()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 57783);

            ConsoleController.Log("ip " + localEndPoint.ToString(), "Socket Manager");

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10);

            listener.BeginAccept(new AsyncCallback(AcceptCallBack), listener);
            ConsoleController.Log("Accepting...", "Socket Manager");
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            Socket clientSocket = listener.EndAccept(ar);

            ConsoleController.Log("Connected : " + clientSocket.RemoteEndPoint.ToString(), "Socket Manager");

            FlowClient client = new FlowClient(clientSocket, DisconnectClient);
            clients.Add(client);

            ConsoleController.UpdateConnectionList(clients);

            listener.BeginAccept(new AsyncCallback(AcceptCallBack), listener);
            ConsoleController.Log("Accepting...", "Socket Manager");
        }

        protected void DisconnectClient(FlowClient client)
        {
            //Client 접속 인터페이스를 만들어서 컨트롤할 수 있도록 수정
            clients.Remove(client);
        }
    }
}
