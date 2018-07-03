using FlowServer.Server.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowServer.Server.FlowServices.Mud
{
    public enum GameCommandType { GENERAL, OPTION, INPUT, SYSTEM, NONE }

    public sealed class GameCommand
    {
        public GameCommandType Type = GameCommandType.NONE;
        public IFlowClient Target = null;
        public string Title = "";
        public string Command = "";
    }
}
