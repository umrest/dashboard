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
                client.ConnectAsync("localhost", 8091).Wait(100);

                byte[] identifier = new byte[128];
                identifier[0] = 250;
                identifier[1] = 1;
                client.Client.Send(identifier);
            }
            catch
            {

            }
           
        }

        public bool connected()
        {
           return client.Connected;
        }

        public void send(byte[] bytes)
        {
            
            if (!client.Connected)
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
                return false;
            }
            if (client.Available >= bytes.Length)
            {
                client.GetStream().Read(bytes, 0, bytes.Length);
                return true;
            }

            return false;
        }
    }
}
