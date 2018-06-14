using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowServer.Server.Core
{
    public delegate void ConsoleControllerPrintHandler(string agent, string text, PrintType type);
    public delegate void ConsoleControllerBroadcastHandler(string message);
    public enum PrintType { NORMAL, DEBUGING, WARNING, ERROR }

    public class ConsoleController
    {
        private static event ConsoleControllerPrintHandler PrintEvent;
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


        public static void MessageBroadcast(string message)
        {
            BroadcastEvent(message);
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

    }
}
