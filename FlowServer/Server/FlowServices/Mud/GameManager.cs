using FlowServer.Server.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FlowServer.Server.FlowConsole;

namespace FlowServer.Server.FlowServices.Mud
{
    class GameManager
    {
        private readonly int channel = 0;
        private readonly Thread thread;
        private readonly Queue<string> clientRequestList;

        private readonly GameEnvironment environment;
        private readonly Dictionary<IFlowClient, GamePlayer> players;

        public GameManager(int channel)
        {
            this.channel = channel;
            clientRequestList = new Queue<string>();
            environment = new GameEnvironment();
            players = new Dictionary<IFlowClient, GamePlayer>();

            thread = new Thread(Initialize);
            thread.Start();
        }

        public void Initialize()
        {
            ConsoleController.Log("Game " + channel + " Initializing Complete");
        }

        public void ReceiveGameMessage(string message, IFlowClient client)
        {
            ConsoleController.Debug("Game" + channel + " " + message);
        }
    }
}
