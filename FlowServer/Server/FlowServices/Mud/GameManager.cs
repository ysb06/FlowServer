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
        private readonly Queue<GameCommand> clientRequestList;
        

        private Timer workThreadTimer;
        private const int updateRate = 0;

        private readonly GameEnvironment environment;
        private readonly Dictionary<IFlowClient, GamePlayer> players;

        public GameManager(int channel)
        {
            this.channel = channel;
            clientRequestList = new Queue<GameCommand>();

            environment = new GameEnvironment();
            players = new Dictionary<IFlowClient, GamePlayer>();

            workThreadTimer = new Timer(Loop);
            workThreadTimer.Change(0, 17);
            
            ConsoleController.Log("Games " + channel + " Initializing Complete");
        }

        public void Loop(object state)
        {
            while (clientRequestList.Count > 0)
            {
                GameCommand req = clientRequestList.Dequeue();
                Console.Write(req.Title + ", ");
                Console.WriteLine(req.Command);
            }
        }

        public void AddPlayer(IFlowClient client)
        {
            players.Add(client, new GamePlayer());
            GameCommand request = new GameCommand
            {
                Type = GameCommandType.SYSTEM,
                Target = client,
                Title = "Send_List"
            };
            clientRequestList.Enqueue(request);

        }

        public void ReceiveGameMessage(string message, IFlowClient client)
        {
            if (message[1] == 'I')
            {
                string[] str = message.Split('^');

                GameCommand request = new GameCommand
                {
                    Type = GameCommandType.INPUT,
                    Target = client,
                };

                if(str.Length == 1)
                {
                    request.Command = str[0].Remove(0, 2);
                    clientRequestList.Enqueue(request);
                }
                else if(str.Length > 1)
                {
                    request.Title = str[0].Remove(0, 2);
                    request.Command = str[1];
                    clientRequestList.Enqueue(request);
                }
            }
        }
    }
}
