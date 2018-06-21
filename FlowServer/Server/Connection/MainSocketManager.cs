using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using FlowServer.Server.FlowConsole;

namespace FlowServer.Server.Connection
{
    /// <summary>
    /// 클라이언트 접속 관리. 클라이언트 요청 수락 및 클라이언트 객체 관리
    /// </summary>
    class MainSocketManager
    {
        private Thread threadMain;

        private readonly List<FlowClient> clients = new List<FlowClient>();

        public MainSocketManager()
        {
            ConsoleController.Broadcast += ConsoleController_Broadcast;
            threadMain = new Thread(Initialize);
            threadMain.Start();
        }

        /// <summary>
        /// 콘솔 메시지에 따른 처리 정의
        /// </summary>
        /// <param name="message">메시지 원본</param>
        /// <param name="convertedArgs">요소별로 나눈 메시지 원본</param>
        private void ConsoleController_Broadcast(string message, string[] convertedArgs)
        {
            Console.WriteLine("Broadcast : " + convertedArgs[0]);

            try
            {
                if (convertedArgs[0].Equals("disconnect"))
                {
                    FlowClient _target = null;

                    foreach (FlowClient client in clients)
                    {
                        if (convertedArgs[1].Equals(client.ToString()))
                        {
                            _target = client;
                        }
                    }

                    if(_target == null)
                    {
                        ConsoleController.Alert(convertedArgs[0] + " 대상이 잘못되었습니다.");
                    }
                    else
                    {
                        DisconnectClient(_target);
                    }
                }
                else if (convertedArgs[0].Equals("send"))
                {
                    foreach (FlowClient client in clients)
                    {
                        if (convertedArgs[1].Equals(client.ToString()))
                        {
                            ConsoleController.Debug("Sending(" + client + ") : " + convertedArgs[2]);
                            client.SendMessage(convertedArgs[2]);
                        }
                    }
                }
            }
            catch(IndexOutOfRangeException)
            {
                ConsoleController.Alert(convertedArgs[0] + " 명령어 인수가 부족합니다.");
            }
        }

        private void Initialize()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 57783);

            ConsoleController.Log("ip " + localEndPoint.ToString());

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10);

            listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
            ConsoleController.Log("Accepting...");
        }

        /// <summary>
        /// 비동기 소켓 Accept을 위한 Callback
        /// </summary>
        /// <param name="ar">비동기 호출 결과</param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            Socket clientSocket = listener.EndAccept(ar);

            ConsoleController.Debug("Connected : " + clientSocket.RemoteEndPoint.ToString());

            FlowClient client = new FlowClient(clientSocket);
            client.MessageReceived += Client_MessageReceived;
            client.Disconnected += DisconnectClient;
            clients.Add(client);

            List<IFlowClient> iClients = new List<IFlowClient>(clients);
            ConsoleController.UpdateConnectionList(iClients);

            listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
            ConsoleController.Log("Accepting...");
        }


        private void Client_MessageReceived(string result, IFlowClient client)
        {
            ConsoleController.Log(result, client.ToString());
        }

        private void DisconnectClient(FlowClient client)
        {
            client.Dispose();

            ConsoleController.Warn("Disconnected : " + client.ToString());
            //Client 접속 인터페이스를 만들어서 컨트롤할 수 있도록 수정
            clients.Remove(client);

            List<IFlowClient> iClients = new List<IFlowClient>(clients);
            ConsoleController.UpdateConnectionList(iClients);
        }
    }
}
