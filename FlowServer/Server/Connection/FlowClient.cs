﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FlowServer.Server.Connection
{
    public delegate void ClientMessageHandler(string result, IFlowClient client);
    public delegate void ClientDisconnectionHandler(FlowClient client);
    public delegate void ClientConnectionHandler(IFlowClient client);

    /// <summary>
    /// 클라이언트 객체
    /// </summary>
    public class FlowClient : IFlowClient
    {
        private event ClientMessageHandler MessageReceivedEvent;
        public event ClientMessageHandler MessageReceived
        {
            add { MessageReceivedEvent += value;}
            remove { MessageReceivedEvent -= value; }
        }

        private event ClientDisconnectionHandler DisconnectedEvent;
        public event ClientDisconnectionHandler Disconnected
        {
            add { DisconnectedEvent += value; }
            remove { DisconnectedEvent -= value; }
        }

        public const int BUFFER_SIZE = 1024;

        private readonly string name = "No Name";
        private string id = "No ID";

        private readonly Socket connection = null;
        private readonly byte[] buffer = new byte[BUFFER_SIZE];
        private readonly StringBuilder stringBuilder = new StringBuilder();
        private readonly UTF8Encoding stringDecoder;

        public FlowClient(Socket childSocket)
        {
            stringDecoder = new UTF8Encoding();

            connection = childSocket;
            connection.Send(Encoding.UTF8.GetBytes("Server : Accept\r\n"));

            name = connection.RemoteEndPoint.ToString();
            GenerateID();

            connection.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveMessageCallback), null);
        }

        /// <summary>
        /// 비동기 소켓 메시지를 받기 위한 Callback
        /// </summary>
        /// <param name="result">비동기 호출 결과</param>
        protected void ReceiveMessageCallback(IAsyncResult result)
        {
            if(!connection.Connected)
            {
                Console.WriteLine(name + "is disconnected");
                return;
            }

            int readNum = connection.EndReceive(result);
                        
            //버퍼가 가득 찰 정도로 0을 보낼 경우 클라이언트의 연결이 끊어졌다고 판단하여 Client를 Dispose
            for(int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != 0)
                {
                    break;
                }

                if(i == buffer.Length - 1)
                {
                    Dispose();
                    DisconnectedEvent(this);
                    return;
                }
            }

            MessageReceivedEvent(stringDecoder.GetString(buffer, 0, readNum), this);
            connection.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveMessageCallback), null);
        }

        public override string ToString()
        {
            if (connection == null)
            {
                return "Null Connection";
            }
            else
            {
                return name;
            }
        }

        public Socket GetSocket()
        {
            return connection;
        }

        private void Dispose()
        {
            connection.Disconnect(true);
            connection.Close();            
            connection.Dispose();
        }

        public void SendMessage(string message)
        {
            byte[] temp = Encoding.UTF8.GetBytes(message);
            connection.Send(temp);
        }

        public void SendMessage(byte[] message)
        {
            connection.Send(message);
        }

        private void GenerateID()
        {
            StringBuilder builder = new StringBuilder(name);
            builder.Append(DateTime.Now);

            byte[] result = HashAlgorithm.Create().ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));
            string str = "";

            for(int i = 0; i < result.Length; i++)
            {
                str += BitConverter.ToString(result, i, 1).ToLower();
            }

            id = str;
        }

        public string GetUniqueHash()
        {
            return id;
        }
    }
}
