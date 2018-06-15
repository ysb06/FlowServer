using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlowServer.Server.Connection
{
    class ClientObject
    {
        public const int BUFFER_SIZE = 1024;

        public Socket connectedSocket = null;
        public byte[] buffer = new byte[BUFFER_SIZE];
        public StringBuilder stringBuilder = new StringBuilder();
    }
}
