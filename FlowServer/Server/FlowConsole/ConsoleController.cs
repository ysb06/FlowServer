using FlowServer.Server.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowServer.Server.FlowConsole
{
    public delegate void ConsoleControllerPrintHandler(string agent, string text, PrintType type);
    public delegate void ConsoleControllerBroadcastHandler(string message, string[] convertedArgs);
    public enum PrintType { NORMAL, DEBUGING, WARNING, ERROR }

    public delegate void ConsoleControllerConnectionHandler(List<IFlowClient> list);

    public class ConsoleController
    {
        private static event ConsoleControllerPrintHandler PrintEvent;
        /// <summary>
        /// 콘솔 메시지 표시 요청이 왔을 때
        /// </summary>
        public static event ConsoleControllerPrintHandler Print
        {
            add
            {
                PrintEvent += value;
            }
            remove
            {
                PrintEvent -= value;
            }
        }
        private static event ConsoleControllerBroadcastHandler BroadcastEvent;
        /// <summary>
        /// 관리자가 명령어를 입력했을 때
        /// </summary>
        public static event ConsoleControllerBroadcastHandler Broadcast
        {
            add
            {
                BroadcastEvent += value;
            }
            remove
            {
                BroadcastEvent -= value;
            }
        }

        private static event ConsoleControllerConnectionHandler UpdateConnectionEvent;
        /// <summary>
        /// 새로운 연결이 성립되어 리스트 업데이트 요청이 왔을 때
        /// </summary>
        public static event ConsoleControllerConnectionHandler UpdateConnection
        {
            add
            {
                UpdateConnectionEvent += value;
            }
            remove
            {
                UpdateConnectionEvent -= value;
            }
        }


        public static void MessageBroadcast(string message)
        {
            string[] args = message.Split(' ');
            args[0] = args[0].ToLower();
            BroadcastEvent(message, args);
        }

        public static void Log(string content, string agent = "Undefined Agent")
        {
            PrintEvent(agent, content, PrintType.NORMAL);
        }

        public static void Debug(string content, string agent = "Undefined Agent")
        {
            PrintEvent(agent, content, PrintType.DEBUGING);
        }

        public static void Warn(string content, string agent = "Undefined Agent")
        {
            PrintEvent(agent, content, PrintType.WARNING);
        }

        public static void Alert(string content, string agent = "Undefined Agent")
        {
            PrintEvent(agent, content, PrintType.ERROR);
        }


        public static void UpdateConnectionList(List<IFlowClient> list)
        {
            UpdateConnectionEvent(list);
        }
    }
}
