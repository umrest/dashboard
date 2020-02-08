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

        ThreadStart send_joystick_ts;
        Thread send_joystick_thread;

        bool socket_connected = false;

        string host = "192.168.61.128";//"192.168.0.120";
        int port = 8091;

        byte[] buffer = new byte[128000];

        public bool connected()
        {
            return socket_connected;
        }


        public CommunicationHandlerNew(MainWindow parent_in)
        {
            parent = parent_in;

            send_joystick_ts = new ThreadStart(send_joystick_data);

            socket_reconnect();
        }

        public void send_joystick_data()
        {
            Joystick stick = null;

            try
            {
                stick = new SlimDX.DirectInput.Joystick(StateData.Input, StateData.joy_guid);
                stick.Properties.BufferSize = 128;
                if (stick.Acquire().IsFailure)
                {

                    throw new Exception("Joystick Aquire Failed");
                }
            }
            catch
            {

                StateData.send_joystick_enabled = false;
                MessageBox.Show("Failed to Aquire Joystick");
                return;
            }

            while (StateData.send_joystick_enabled)
            {

                if (stick.Poll().IsFailure)
                {
                    return;
                }
                stick.GetBufferedData();
                var state = stick.GetCurrentState();

                StateData.joystick_data.Load(state);

                send(StateData.joystick_data.Serialize());

                System.Threading.Thread.Sleep(50);
            }
        }

        public void socket_reconnect()
        {
            try
            {
                client = new TcpClient();
                client.SendBufferSize = 128;
                client.ReceiveBufferSize = 128000;
                client.BeginConnect(host, port, on_connect, null);
            }
            catch
            {
                System.Threading.Thread.Sleep(100);
                socket_reconnect();
            }
        }

        public void socket_disconnect()
        {
            socket_connected = false;
            Console.WriteLine("Socket: Disconnected");
            
            client.Close();
            client = null;
            on_disconnect();
        }

        public void start_send_joystick()
        {
            StateData.send_joystick_enabled = true;

            if (send_joystick_thread == null || !send_joystick_thread.IsAlive)
            {
                send_joystick_thread = new Thread(send_joystick_ts);
                send_joystick_thread.Start();
            }
        }

        public void stop_send_joystick()
        {
            StateData.send_joystick_enabled = false;
        }

        void socket_read()
        {
            cur_offset = 0;
            socket_read_key();
        }

        public void on_key(IAsyncResult ar)
        {
            try
            {

                   int bytesRead = client.GetStream().EndRead(ar);

                if (bytesRead + cur_offset != 3)
                {

                    if (socket_connected)
                    {
                        if (bytesRead > 0)
                        {
                            cur_offset += bytesRead;
                            socket_read_key();
                        }
                        else
                        {
                            socket_disconnect();
                            socket_reconnect();
                        }

                    }
                    return;
                }

                cur_offset = 0;

                for (int i = 0; i < 3; i++)
                    {
                        if (buffer[i] != CommunicationDefinitions.key[i])
                        {
                            Console.WriteLine("Invalid key");
                        socket_read();
                        return;
                        }
                    }
                    // Valid key
                    //Console.WriteLine("Valid key");
                    socket_read_header();

                
            }
            catch
            {
                if (socket_connected)
                {
                    socket_disconnect();
                    socket_reconnect();
                }
            }
        }


        public void socket_read_header()
        {
            try
            {
                client.GetStream().BeginRead(buffer, 0, 1, new AsyncCallback(on_header), null);
            }
            catch
            {
                if (socket_connected)
                {
                    socket_disconnect();
                    socket_reconnect();
                }
            }
            
        }

        public void socket_read_key()
        {
            try
            {
               client.GetStream().BeginRead(buffer, cur_offset, 3, new AsyncCallback(on_key), null);
            }
            catch
            {
                if (socket_connected)
                {
                    socket_disconnect();
                    socket_reconnect();
                }
            }
        }

        int read_size = 0;
        int cur_offset = 0;
        public void socket_read_data()
        {
            if (CommunicationDefinitions.PACKET_SIZES.ContainsKey((CommunicationDefinitions.TYPE)buffer[0]))
            {
                read_size = CommunicationDefinitions.PACKET_SIZES[(CommunicationDefinitions.TYPE)buffer[0]];
                client.GetStream().BeginRead(buffer, 1 + cur_offset, read_size - cur_offset, new AsyncCallback(on_recv), null);

            }
            else
            {
                Console.WriteLine("Invalid Type");
                
                socket_read_header();
            }
        }

        public void on_recv(IAsyncResult ar) 
        {

            try { 
                    int bytesRead = client.GetStream().EndRead(ar);

                    if (bytesRead + cur_offset != read_size)
                    {
                        
                        if (socket_connected)
                        {
                            if(bytesRead > 0)
                            {
                                cur_offset += bytesRead;
                                socket_read_data();
                            }
                            else
                            {
                                socket_disconnect();
                                socket_reconnect();
                            }
                            
                        }
                        return;
                    }


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
                    else if (type == CommunicationDefinitions.TYPE.SENSOR_STATE)
                    {
                        StateData.sensor_state_data.Deserialize(buffer);
                    }
                    else if (type == CommunicationDefinitions.TYPE.VISION_IMAGE)
                    {
                        parent.vision_view.SetImage(buffer.Skip(1).ToArray());
                    }
                    else if (type == CommunicationDefinitions.TYPE.ROBOT_STATE)
                    {
                        StateData.robot_state_data.Deserialize(buffer);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Type");
                    }

                    socket_read();

                
            }
            catch
            {
                if (socket_connected)
                {
                    socket_disconnect();
                    socket_reconnect();
                }
            }
        }

        public void send(byte[] buf)
        {
            try
            {
                List<byte> list = new List<byte>();
                list.AddRange(CommunicationDefinitions.key);
                list.AddRange(buf);

                byte[] data = list.ToArray();
                client.GetStream().BeginWrite(data, 0, data.Length, on_send, client.Client);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                if (socket_connected)
                {
                    socket_disconnect();
                    socket_reconnect();
                }
            }
        }

        public void on_header(IAsyncResult ar)
        {
            try
            {
                
                    int bytesRead = client.GetStream().EndRead(ar);

                    if (bytesRead != 1)
                    {
                        if (socket_connected)
                        {
                            socket_disconnect();
                            socket_reconnect();
                        }
                        return;
                    }

                    socket_read_data();
                
            }
            catch
            {
                if (socket_connected)
                {
                    socket_disconnect();
                    socket_reconnect();
                }
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
                if (socket_connected)
                {
                    socket_disconnect();
                    socket_reconnect();
                }
            }
        }

        private void on_connect(IAsyncResult ar)
        {
            try
            {

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.Client.RemoteEndPoint.ToString());

                Console.WriteLine("Socket: Reconnect Succeeded");
                socket_connected = true;
                socket_read();

                

                byte[] identifier = new byte[128];
                identifier[0] = (byte)CommunicationDefinitions.TYPE.INDENTIFIER;
                identifier[1] = (byte)CommunicationDefinitions.IDENTIFIER.DASHBOARD;

                send(identifier);
            }
            catch (Exception e)
            {
                Console.WriteLine("Socket: Reconnect Failed");
                Console.WriteLine(e);
                    socket_disconnect();
                    socket_reconnect();
                
            }
            parent.update_indicators();
        }

        private void on_disconnect()
        {
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
