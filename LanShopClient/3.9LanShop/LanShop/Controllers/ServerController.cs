using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;

namespace LanShop.Controllers
{
    class ServerController : Controller
    {
        static Vst.Network.AsyncServer _server;
        public ActionResult Start()
        {            
            if (_server == null)
            {
                _server = new Vst.Network.AsyncServer();
                _server.RequestReceived += (e) => {
                    var msg = new Vst.Network.SocketMessage(e);
                    e.Response = Vst.Json.GetString(Vst.Network.SocketMvc.ProcessRequest(msg));
                };
            }

            _server.StartListening("192.168.0.101");
            return Done();
        }
    }
}
