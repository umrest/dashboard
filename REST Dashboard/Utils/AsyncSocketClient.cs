using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    class AsyncSocketClient
    {
        public TcpClient client = null;

        static readonly object _lock = new object();

        private bool _connected = false;
        public AsyncSocketClient()
        {
            connect();
        }


        public void connect()
        {
            lock (_lock)
            {
                if (client != null)
                {
                    client.Close();

                }
                client = new TcpClient();
                client.SendBufferSize = 128;
                client.SendTimeout = 100;
                client.ReceiveTimeout = 100;
                try
                {
                    // 192.168.0.120
                    client.ConnectAsync("127.0.0.1", 8091).Wait(1000);
                    if (client.Connected)
                    {
                        byte[] identifier = new byte[128];
                        identifier[0] = 250;
                        identifier[1] = 1;
                        client.Client.Send(identifier);
                    }
                }
                catch
                {

                }
            }
           
        }

        public bool connected()
        {
            lock (_lock)
            {


                if (client == null || client.Client == null || client.Connected == false)
                {
                    return false;
                }
                try
                {
                    return !(client.Client.Poll(1, SelectMode.SelectRead) && client.Client.Available == 0);
                }
                catch { return false; }
            }
        }

        public void send(byte[] bytes)
        {
            
            if (!connected())
            {
                connect();
            }
            lock (_lock)
            {
                try
                {
                    client.Client.Send(bytes);
                }
                catch
                {
                }
            }
        }

        public bool recieve(List<byte[]> messages)
        {
            if (!connected())
            {
                connect();
                return false;
            }
            lock (_lock)
            {
                //c Console.WriteLine("Before: {0}" , client.Available);
                while (client.Available >= 128)
                {
                    messages.Add(new byte[128]);
                    client.Client.Receive(messages.Last(), 128, SocketFlags.None);
                }
                // Console.WriteLine("After:  {0}", client.Available);

                return true;
            }
        }
    }
}
