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
            try
            {
                client.ConnectAsync("localhost", 8091);
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
                client.GetStream().Write(bytes, 0, bytes.Length);
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
