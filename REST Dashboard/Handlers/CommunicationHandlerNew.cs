using REST_Dashboard.CommunicationStandards;
using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace REST_Dashboard.Handlers
{
    public class CommunicationHandlerNew
    {

        TcpClient client;

        MainWindow parent;

        bool socket_connected = false;

        string host = "uofmrestraspberrypi";
        int port = 8091;

        byte[] buffer = new byte[128000];

        public bool connected()
        {
            return socket_connected;
        }


        public CommunicationHandlerNew(MainWindow parent_in)
        {
            parent = parent_in;

            socket_reconnect();
        }

        public void socket_reconnect()
        {
            client = new TcpClient();
            client.SendBufferSize = 128;
            client.ReceiveBufferSize = 128000;
            client.BeginConnect(host, port, on_connect, client.Client);
        }

        public async void socket_read_header()
        {
            await client.GetStream().ReadAsync(buffer, 0, 1);
            on_header();
        }

        public async void socket_read_data()
        {
            if (CommunicationDefinitions.PACKET_SIZES.ContainsKey((CommunicationDefinitions.TYPE)buffer[0]))
            {
                int size = CommunicationDefinitions.PACKET_SIZES[(CommunicationDefinitions.TYPE)buffer[0]];
                int x = client.GetStream().Read(buffer, 1 , size);
                on_recv();
            }
            else
            {
                Console.WriteLine("Invalid Type");
                
                socket_read_header();
            }
        }

        public void on_recv() 
        {
            try
            {
                

                // Signal that all bytes have been sent.  
                socket_connected = true;

                CommunicationDefinitions.TYPE type = (CommunicationDefinitions.TYPE)buffer[0];

                if (type == CommunicationDefinitions.TYPE.DATAAGGREGATOR_STATE)
                {
                    StateData.dataaggregator_state.Deserialize(buffer);

                    parent.update_indicators();
                }
                else if (type == CommunicationDefinitions.TYPE.VISION)
                {
                    StateData.vision_data.Deserialize(buffer);
                }
                else if (type == CommunicationDefinitions.TYPE.ROBOT_STATE)
                {
                    StateData.robot_state_data.Deserialize(buffer);
                }
                else if (type == CommunicationDefinitions.TYPE.VISION_IMAGE)
                {
                    parent.vision_view.SetImage(buffer.Skip(1).ToArray());
                }
                else
                {
                    Console.WriteLine("Invalid Type");
                }

                socket_read_header();

            }
            catch (Exception e)
            {
                Console.WriteLine("Socket: Recv Failed");
                socket_connected = false;
                socket_reconnect();
            }
        }

        public void send(byte[] buf)
        {
            client.GetStream().BeginWrite(buf, 0, buf.Length, on_send, client.Client);
        }

        public void on_header()
        {
            try
            {  
                socket_connected = true;

                socket_read_data();                
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Socket: Header Recv Failed");
                socket_connected = false;
                socket_reconnect();
            }
        }

        public void on_send(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);

                // Signal that all bytes have been sent.  
                socket_connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Socket: Send Failed");
                socket_connected = false;
                socket_reconnect();
            }
        }

        private void on_connect(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                Console.WriteLine("Socket: Reconnect Succeeded");
                socket_connected = true;
                socket_read_header();

                

                byte[] identifier = new byte[128];
                identifier[0] = (byte)CommunicationDefinitions.TYPE.INDENTIFIER;
                identifier[1] = (byte)CommunicationDefinitions.IDENTIFIER.DASHBOARD;

                send(identifier);
            }
            catch (Exception e)
            {
                Console.WriteLine("Socket: Reconnect Failed");
                socket_connected = false;
                socket_reconnect();
            }
            parent.update_indicators();
        }

        public void send_vision_properties()
        {
            byte[] data = StateData.properties.Serialize();

            send(data);
        }

        public void send_dashboard_state()
        {
            send(StateData.dashboard_state.Serialize());
        }

        public void send_vision_image()
        {
            byte[] data = new byte[128];
            data[0] = 12;
            data[1] = 0;
            send(data);
        }
    }
}
