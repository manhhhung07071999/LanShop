using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;
using Vst.Network;
using System.Threading;

namespace LanShop.Controllers
{
    class ServerController : Controller
    {
        public ActionResult Default()
        {
            var config = SetupController.Config;
            var ip = config.ServerIp;

            if (ip == null)
            {
                return Redirect("setup");
            }

            var server = new AsyncServer();
            server.RequestReceived += (e) => {
                var msg = new SocketMessage(e);

                Console.WriteLine(msg.Topic);
                e.Response = Vst.Json.GetString(SocketMvc.ProcessRequest(msg));
            };

            MyApp.BeginInvoke(() => {
                server.StartListening(ip, (e) => {
                    SetupController.Config.ServerIp = null;
                });
            });

            Thread.Sleep(200);
            if (SetupController.Config.ServerIp == null)
            {
                return View(new Views.Server.Error(), ip);
            }

            return Redirect("hoaDon");
        }
    }
}
