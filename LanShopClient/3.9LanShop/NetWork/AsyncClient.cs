using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace Vst.Network
{
    class AsyncEvent
    {
        public const int Connect = 0;
        public const int Send = 1;
        public const int Receive = 2;

        ManualResetEvent[] _events;
        public AsyncEvent()
        {
            _events = new ManualResetEvent[3];
            for (int i = 0; i < _events.Length; i++)
            {
                _events[i] = new ManualResetEvent(false);
            }
        }
        public void Set(int index)
        {
            _events[index].Set();
        }
        public void Wait(int index)
        {
            _events[index].WaitOne();
        }
    }

    public class AsyncClient : AsyncSocket
    {
        // ManualResetEvent instances signal completion.  
        private AsyncEvent asyncEvent;
        // The response from the remote device.  
        public event Action<ResponseReceivedEventArgs> ResponseReceived;
        public event Action ConnectionError;
        public event Action SendingError;
        public event Action ReceivingError;

        public void Request(string ip, string message, Action callback)
        {
            // Connect to a remote device.  
            try
            {
                asyncEvent = new AsyncEvent();

                // Establish the remote endpoint for the socket.  
                // The name of the
                // remote device is "host.contoso.com".  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ip == null ? ipHostInfo.AddressList[0] : IPAddress.Parse(ip);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, Port);

                Socket = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                Socket.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), Socket);
                asyncEvent.Wait(AsyncEvent.Connect);

                // Send test data to the remote device.  
                Send(Socket, message);
                asyncEvent.Wait(AsyncEvent.Send);

                // Receive the response from the remote device.  
                Receive(Socket);
                asyncEvent.Wait(AsyncEvent.Receive);

                // Release the socket.  
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();

                callback?.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                // Signal that the connection has been made.  
                asyncEvent.Set(AsyncEvent.Connect);
            }
            catch (Exception e)
            {
                ConnectionError?.Invoke();
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                // Begin receiving the data from the remote device.  
                client.BeginReceive(_buffer, 0, this.Capacity, 0,
                    new AsyncCallback(ReceiveCallback), this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                // Read data from the remote device.  
                int bytesRead = Socket.EndReceive(ar);
                var done = false;

                if (bytesRead > 0)
                {
                    done = IsAllReceived(bytesRead);
                }
                
                if (!done)
                {
                    Socket.BeginReceive(_buffer, 0, this.Capacity, 0,
                        new AsyncCallback(ReceiveCallback), this);
                }
                else
                {
                    ResponseReceived?.Invoke(new ResponseReceivedEventArgs { Response = GetString() });
                    asyncEvent.Set(AsyncEvent.Receive);
                }
            }
            catch (Exception e)
            {
                ReceivingError?.Invoke();
            }
        }

        private void Send(Socket client, string data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            var sendData = GetSendingData(data);

            // Begin sending the data to the remote device.  
            client.BeginSend(sendData, 0, sendData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);

                // Signal that all bytes have been sent.  
                asyncEvent.Set(AsyncEvent.Send);
            }
            catch (Exception e)
            {
                SendingError?.Invoke();
            }
        }
    }
}