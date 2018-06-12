using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlowServer.Server.Core
{
    class MainThreadController
    {
        private Thread threadMain;

        public MainThreadController()
        {
            threadMain = new Thread(run);
            threadMain.Start();
        }

        private void run()
        {

        }
    }
}
