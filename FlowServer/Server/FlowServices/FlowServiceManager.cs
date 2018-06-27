using FlowServer.Server.Connection;
using FlowServer.Server.FlowServices.Mud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowServer.Server.FlowConsole;

namespace FlowServer.Server.FlowServices
{
    class FlowServiceManager
    {
        private readonly GameManager[] game;
        public FlowServiceManager(int gameServerNumber)
        {
            game = new GameManager[gameServerNumber];
            for(int i = 0; i < gameServerNumber; i++)
            {
                game[i] = new GameManager(i);
            }
            ConsoleController.Log("Service Initializing Complete");
        }

        public void Service_ReceiveMessage(string message, IFlowClient client)
        {
            if (message != null && message == "")
            {
                if (message[0] == 'G')
                {
                    foreach (GameManager manager in game)
                    {
                        manager.ReceiveGameMessage(message, client);
                    }
                }
            }
        }

        public void Service_ClientConnected(IFlowClient client)
        {

        }
    }
}
