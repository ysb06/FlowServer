using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlowServer.Server.Connection
{
    public class FlowClient
    {
        public const int BUFFER_SIZE = 1024;

        private readonly Socket connection = null;
        private readonly byte[] buffer = new byte[BUFFER_SIZE];
        private readonly StringBuilder stringBuilder = new StringBuilder();

        private readonly ClientHandler handler;

        public FlowClient(Socket socket, ClientHandler handler)
        {
            connection = socket;
            this.handler = handler;
        }

        private void Dispose()
        {
            connection.Dispose();
            handler(this);
        }
    }
}
