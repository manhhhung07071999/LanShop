using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVC = System.Mvc;

namespace DataServer.Controllers
{
    class Controller : Vst.Network.SocketController
    {
        protected override void OnExecuteError(Exception e)
        {
            base.OnExecuteError(e);
        }
    }
}
