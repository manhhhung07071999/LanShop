using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vst.Network;

namespace DataServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Models.Data.RemovableDB.Create(args.Length == 0 ? "e:\\" : args[1]);

            SocketMvc.Register(new Program(), result => { });

            var server = new AsyncServer();
            server.RequestReceived += (e) => {
                var msg = new SocketMessage(e);

                Console.WriteLine(msg.Topic);
                e.Response = Vst.Json.GetString(SocketMvc.ProcessRequest(msg));

            };

            var ip = args.Length == 0 ? "192.168.1.3" : args[0];
            server.StartListening(ip);
        }
    }
}
