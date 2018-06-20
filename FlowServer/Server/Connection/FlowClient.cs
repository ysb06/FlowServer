using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlowServer.Server.Connection
{
    public class FlowClient : IFlowClient
    {
        public const int BUFFER_SIZE = 1024;

        private readonly Socket connection = null;
        private byte[] buffer = new byte[BUFFER_SIZE];
        private readonly StringBuilder stringBuilder = new StringBuilder();

        private readonly ClientHandler handler;

        public FlowClient(Socket childSocket, ClientHandler handler)
        {
            connection = childSocket;
            buffer = Encoding.UTF8.GetBytes("Server : Accept");
            connection.Send(buffer);
            this.handler = handler;
        }

        public override string ToString()
        {
            if (connection == null)
            {
                return "Null Connection";
            }
            else
            {
                return connection.RemoteEndPoint.ToString();
            }
        }

        public void Dispose()
        {
            connection.Disconnect(false);
            connection.Close();
            connection.Dispose();
            handler(this);
        }

        public void SendMessage(string message)
        {
            buffer = Encoding.UTF8.GetBytes(message);
            connection.Send(buffer);
        }

        public void SendMessage(byte[] message)
        {
            connection.Send(message);
        }
    }
}
