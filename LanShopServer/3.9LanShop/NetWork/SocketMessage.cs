using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vst.Network
{
    public class SocketMessage
    {
        public int Code { get; set; }
        public String Topic { set; get; }
        public String Message { set; get; }

        public SocketMessage() { }
        public SocketMessage(string topic)
        {
            Topic = topic;
        }
        public SocketMessage(string topic, string msg)
        {
            Topic = topic;
            Message = msg;
        }
        public SocketMessage(string topic, object value)
        {
            Topic = topic;
            Message = Vst.Json.GetString(value);
        }

        public SocketMessage(RequestReceivedEventArgs e)
        {
            var sm = Vst.Json.GetObject<SocketMessage>(e.Request);
            Topic = sm.Topic;
            Message = sm.Message;
        }

        public SocketMessage(ResponseReceivedEventArgs e)
        {
            var sm = Vst.Json.GetObject<SocketMessage>(e.Response);
            Topic = sm.Topic;
            Message = sm.Message;
        }

        public string ToJson()
        {
            return Vst.Json.GetString(this);
        }
    }
}
