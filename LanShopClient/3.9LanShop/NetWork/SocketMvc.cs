using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vst.Network
{
    public class SocketController : System.Mvc.Controller
    {
        public SocketMessage MessageContext { get; set; }
    }
    public class SocketMvc : System.Mvc.Engine
    {
        public static SocketMessage ProcessRequest(SocketMessage msg)
        {
            try
            {
                var context = new System.Mvc.RequestContext(msg.Topic);
                var controller = GetController<SocketController>(context.ControllerName);
                controller.RequestContext = context;
                controller.MessageContext = msg;

                var aname = context.ActionName ?? "Default";
                var method = controller.GetMethod(aname);
                return new SocketMessage(context.ControllerName + "Response/" + aname, 
                    method.Invoke(controller, new object[] { }));
            }
            catch (Exception e)
            {
                return new SocketMessage("error", e.Message);
            }
        }
    }
}
