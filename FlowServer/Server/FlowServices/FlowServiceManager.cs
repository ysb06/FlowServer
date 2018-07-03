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
    /// <summary>
    /// Client 접속을 관리, 적절하게 서비스에 분배 역할
    /// </summary>
    class FlowServiceManager
    {
        private const int GAME_SERVER_NUM = 1;

        private readonly GameManager[] game;
        public FlowServiceManager()
        {
            game = new GameManager[GAME_SERVER_NUM];
            for(int i = 0; i < GAME_SERVER_NUM; i++)
            {
                game[i] = new GameManager(i);
            }
            ConsoleController.Log("Service Initializing Complete");

            
            //TODO: 스레드 생성 코드 생성
        }

        public void Service_ReceiveMessage(string message, IFlowClient client)
        {
            if (message != null && message != "")
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
            //지금은 Channel 0으로 무조건 밀어 넣는 형식
            //채널 1개당 1스레드로 1스레드가 적정 양까지 버틸경우 1채널로 가도 됨
            //그럴 경우 이 서비스 매니저도 필요 없을 듯
            if(game != null && game.Length != 0)
            {
                game[0].AddPlayer(client);
            }
            else
            {
                ConsoleController.Alert("There is no game service", "Service Manager");
            }
        }
    }
}
