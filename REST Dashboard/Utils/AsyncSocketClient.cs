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

        static readonly object send_lock = new object();
        static readonly object recieve_lock = new object();


        private bool _connected = false;
        public AsyncSocketClient()
        {
            connect();
        }


        public void connect()
        {
            lock (recieve_lock) lock(send_lock)
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
                    client.ConnectAsync("192.168.0.120", 8091).Wait(1000);
                    if (client.Connected)
                    {
                        byte[] identifier = new byte[128];
                        identifier[0] = 250;
                        identifier[1] = 1;
                        client.Client.Send(identifier);
                        _connected = true;
                    }
                }
                catch
                {
                    _connected = false;
                }
            }
           
        }

        public bool connected()
        {
            return _connected;
       
        }

        public void send(byte[] bytes)
        {
            
            if (!connected())
            {
                connect();
            }
            lock (send_lock)
            {
                try
                {
                    client.Client.Send(bytes);
                }
                catch
                {
                    _connected = false;
                }
            }
        }

        public bool recieve(List<byte[]> messages)
        {
            if (!connected())
            {
                connect();
            }
            lock (recieve_lock)
            {
                try
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
                catch
                {
                    _connected = false;
                    return false;
                }
                
            }
        }
    }
}
