using REST_Dashboard.CommunicationStandards;
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
            if (connected())
            {
                return;
            }
            lock (recieve_lock) lock(send_lock)
            {
                if (connected())
                {
                    return;
                }

                if (client != null)
                {
                    client.Close();

                }
                client = new TcpClient();
                client.SendBufferSize = 128;
                client.SendTimeout = 10000;
                client.ReceiveTimeout = 10000;
                try
                {
                    // 192.168.0.120
                    client.ConnectAsync("uofmrestraspberrypi", 8091).Wait(1000);
                    if (client.Connected)
                    {
                            _connected = true;

                            byte[] identifier = new byte[128];
                        identifier[0] = (byte)CommunicationDefinitions.TYPE.INDENTIFIER;
                        identifier[1] = (byte)CommunicationDefinitions.IDENTIFIER.DASHBOARD;
                            
                            send(identifier);
                        
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

            return client != null && !(client.Client.Poll(1, SelectMode.SelectRead) && client.Client.Available == 0);


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
                    List<byte> list = new List<byte>();
                    list.AddRange(CommunicationDefinitions.key);
                    list.AddRange(bytes);

                    byte[] data = list.ToArray();
                    client.Client.Send(data);
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
                    while (client.Available >= 1)
                    {
                        byte[] key = new byte[3];

                        client.Client.Receive(key, 3, SocketFlags.None);

                        for(int i = 0; i < 3; i++)
                        {
                            if(key[i] != CommunicationDefinitions.key[i])
                            {
                                Console.WriteLine("Invalid key");
                                continue;
                            }
                        }


                        byte[] t = new byte[1];

                        client.Client.Receive(t, 1, SocketFlags.None);

                        int size = 127; // defualt size

                        CommunicationDefinitions.TYPE type = (CommunicationDefinitions.TYPE)t[0];

                        if(CommunicationDefinitions.PACKET_SIZES.ContainsKey(type))
                        {
                            size = CommunicationDefinitions.PACKET_SIZES[type];
                        }
                        else
                        {
                            Console.WriteLine("invalid type");
                            continue;
                        }

                        byte[] msg = new byte[size + 1];
                        msg[0] = t[0];

                        messages.Add(msg);
                        client.Client.Receive(messages.Last(), 1, size,  SocketFlags.None);
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
