﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlowServer.Server.Connection
{
    public interface IFlowClient
    {
        string ToString();
        string GetUniqueHash();
    }
}

