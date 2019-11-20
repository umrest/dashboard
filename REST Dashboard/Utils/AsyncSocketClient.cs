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

        private bool _connected = false;
        public AsyncSocketClient()
        {
            connect();
        }


        public void connect()
        {
        
            if(client != null)
            {
                client.Close();

            }
            client = new TcpClient();
            client.SendBufferSize = 128;
            client.SendTimeout = 100;
            client.ReceiveTimeout = 100;
            try
            {
                client.ConnectAsync("127.0.0.1", 8091).Wait(100);
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

        public bool connected()
        {
            try
            {
                return !(client.Client.Poll(1, SelectMode.SelectRead) && client.Client.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        public void send(byte[] bytes)
        {
            
            if (!connected())
            {
                connect();
            }
            try
            {
                client.Client.Send(bytes);
            }
            catch
            {
            }
        }

        public bool recieve(ref byte[] bytes)
        {
            if (!connected())
            {
                connect();
            }
            try
            {

                int recieved = client.Client.Receive(bytes, bytes.Length, SocketFlags.None);
                if(recieved == bytes.Length)
                {
                    return true;
                }
                return false;
            }
            catch
            {

            }

            return false;

        }
    }
}
