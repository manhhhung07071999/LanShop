using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Vst.Network
{
    public class RequestReceivedEventArgs : EventArgs
    {
        public string Request { get; set; }
        public string Response { get; set; }
    }
    public class ResponseReceivedEventArgs : EventArgs
    {
        public string Response { get; set; }
    }
    public abstract class AsyncSocket
    {
        // The port number for the remote device.  
        public const int Port = 11000;

        public Socket Socket { get; set; }
        protected byte[] _buffer;

        // Received data string.
        byte[] data;
        int position = -1;

        public int Capacity
        {
            get { return _buffer.Length; }
            set { _buffer = new byte[value]; }
        }

        protected bool IsAllReceived(int bytesRead)
        {
            if (0 >= bytesRead) { return false; }

            int offset = 0;
            if (position < 0)
            {
                int len = 0;

                offset = 4;
                position = 0;
                for (int i = 0; i < offset; i++)
                {
                    len |= (int)_buffer[i] << (i << 3);
                }

                data = new byte[len];
            }

            int k = offset;
            while (position < data.Length && k < bytesRead)
            {
                data[position++] = _buffer[k++];
            }

            var b = position == data.Length;
            if (b) { position = -1; }

            return (b);
        }

        public string GetString()
        {
            return System.Text.Encoding.UTF8.GetString(data);
        }
        public byte[] GetBytes() { return data; }

        protected byte[] GetSendingData(byte[] data)
        {
            var len = data.Length;

            byte[] sendData = new byte[4 + data.Length];
            for (int i = 0; i < 4; i++)
            {
                sendData[i] = (byte)len;
                len >>= 8;
            }
            data.CopyTo(sendData, 4);

            return sendData;
        }
        protected byte[] GetSendingData(string text)
        {
            return GetSendingData(System.Text.Encoding.UTF8.GetBytes(text));
        }

        public AsyncSocket()
        {
            Capacity = 1024;
        }
    }
}
