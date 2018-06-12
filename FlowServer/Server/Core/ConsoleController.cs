using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowServer.Server.Core
{
    public delegate void ConsoleControllerPrintHandler(string agent, string text, PrintType type);
    public enum PrintType { NORMAL, DEBUGING, WARNING, ERROR }

    public class ConsoleController
    {
        public static event ConsoleControllerPrintHandler Print;

        protected static void Register(ConsoleControllerPrintHandler handler)
        {
            Print += handler;
        }

        public static void Log(string content, string agent = "Undefined Agent")
        {
            Print(agent, content, PrintType.NORMAL);
        }

        public static void Debug(string content, string agent = "Undefined Agent")
        {
            Print(agent, content, PrintType.DEBUGING);
        }

        public static void Warn(string content, string agent = "Undefined Agent")
        {
            Print(agent, content, PrintType.WARNING);
        }

        public static void Alert(string content, string agent = "Undefined Agent")
        {
            Print(agent, content, PrintType.ERROR);
        }

    }
}
