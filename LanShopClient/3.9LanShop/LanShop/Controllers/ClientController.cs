using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;
using Vst.Network;

namespace LanShop.Controllers
{
    class Response
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Value { get; set; }
    }

    class ClientController : Controller
    {
        AsyncClient _client;
        public AsyncClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new AsyncClient();
                }
                return _client;
            }
        }

        public void Send(SocketMessage msg, Func<SocketMessage, object> process)
        {
            Client.Request(SetupController.Config.ServerIp, msg.ToJson(), () => {

                try
                {
                    var message = Vst.Json.GetObject<SocketMessage>(_client.GetString());
                    var response = Vst.Json.GetObject<Response>(message.Message);

                    if (response.Code < 0)
                    {
                        return;
                    }

                    if (process != null)
                    {
                        var v = process?.Invoke(message);
                        if (v != null)
                        {
                            MyApp.ExecResponse(v);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}
