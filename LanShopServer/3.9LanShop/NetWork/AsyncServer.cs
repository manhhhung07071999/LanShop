using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Vst.Network
{
    public class AsyncServer : AsyncSocket
    {
        public event Action<RequestReceivedEventArgs> RequestReceived;
        // Thread signal.  
        public ManualResetEvent allDone = new ManualResetEvent(false);

        public void StartListening(string ip, Action<Exception> onError)
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ip == null ? ipHostInfo.AddressList[0] : IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Port);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                Console.WriteLine("Server starting at " + ipAddress);

                listener.Bind(localEndPoint);
                listener.Listen(100);
                
                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
            }
        }

        void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            this.Socket = handler;

            handler.BeginReceive(_buffer, 0, this.Capacity, 0,
                new AsyncCallback(ReadCallback), this);
        }

        void ReadCallback(IAsyncResult ar)
        {
            // Read data from the client socket.
            int bytesRead = Socket.EndReceive(ar);

            //if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                if (IsAllReceived(bytesRead))
                {
                    // Echo the data back to the client.

                    string response = null;
                    if (RequestReceived != null)
                    {
                        var e = new RequestReceivedEventArgs {
                            Request = GetString()
                        };
                        RequestReceived.Invoke(e);

                        response = e.Response;
                    }
                    Send(Socket, response ?? "Done");
                }
                else
                {
                    // Not all data received. Get more.  
                    Socket.BeginReceive(_buffer, 0, this.Capacity, 0,
                        new AsyncCallback(ReadCallback), this);
                }
            }
        }

        private void Send(Socket handler, string data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = GetSendingData(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}