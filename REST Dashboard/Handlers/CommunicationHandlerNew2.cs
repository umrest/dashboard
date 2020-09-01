using REST_Dashboard.CommunicationStandards;
using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace REST_Dashboard.Handlers
{
    
    public class CommunicationHandlerNew2
    {

        TcpClient client;

        MainWindow parent;

        ThreadStart connection_ts;
        Thread connection_thread;

        bool socket_connected = false;

        string host = "192.168.0.120";//"uofmrestraspberrypi"; //"192.168.61.128";//;
        int port = 8091;

        byte[] recieve_buffer = new byte[128000];


        List<byte[]> send_buffer = new List<byte[]>();

        int recieve_size = 0;

        public enum ConnectionState{
            Disconnected = 0, // Not connected
            Connected = 1, // Send
            
        }

        public enum RecieveState
        {
            Key = 0,
            Header = 1,
            Data = 2
        }

        ConnectionState connection_state = ConnectionState.Disconnected;
        RecieveState recieve_state = RecieveState.Key;


        public bool connected()
        {
            return connection_state != ConnectionState.Disconnected;
        }

        public CommunicationHandlerNew2(MainWindow parent_in)
        {
            parent = parent_in;

            connection_ts = new ThreadStart(connection);
            connection_thread = new Thread(connection_ts);
            connection_thread.Start();
        }



        public void connection()
        {
            while (true)
            {
                switch (connection_state)
                {
                    case ConnectionState.Disconnected:
                        socket_reconnect();
                        break;
                    case ConnectionState.Connected:
                        connected_handler();
                        break;
                    default:
                        Console.WriteLine("invalid state");
                        break;
                }
            }
        }

        DateTime last_joystick_send = DateTime.Now;
        DateTime last_heartbeat_send = DateTime.Now;
        DateTime last_state_send = DateTime.Now;
        public void connected_handler()
        {
            var now = DateTime.Now;
            if(StateData.send_joystick_enabled && (now - last_joystick_send).TotalMilliseconds > 50)
            {
                send_joystick();
                last_joystick_send = now;
            }

            if ( (now - last_heartbeat_send).TotalMilliseconds > 500)
            {
                send_identifier();
                last_heartbeat_send = now;
            }

            if ((now - last_state_send).TotalMilliseconds > 100)
            {
                send_dashboard_state();
                last_state_send = now;
            }

            send_buffered();

            recieve_buffered();

       
        }

        public void recieve_buffered()
        {
            if (connection_state != ConnectionState.Connected)
            {
                return;
            }

            while (client.Available > 0)
            {

                if (recieve_state == RecieveState.Key)
                {
                    if (client.Available < 3)
                    {
                        break;
                    }
                    byte[] cur_key = new byte[3];
                    if (client.GetStream().Read(cur_key, 0, 3) == 3)
                    {
                        // Got key, check it
                        bool valid_key = true;
                        for (int i = 0; i < 3; i++)
                        {
                            if (cur_key[0] != CommunicationDefinitions.key[0])
                            {
                                valid_key = false;
                            }
                        }

                        if (valid_key)
                        {
                            recieve_state = RecieveState.Header;
                        }

                        else
                        {

                            recieve_state = RecieveState.Key;
                        }


                    }
                }
                else if (recieve_state == RecieveState.Header)
                {
                    if (client.Available < 1)
                    {
                        break;
                    }

                    if (client.GetStream().Read(recieve_buffer, 0, 1) == 1)
                    {

                        if (CommunicationDefinitions.PACKET_SIZES.ContainsKey((CommunicationDefinitions.TYPE)recieve_buffer[0]))
                        {
                            recieve_size = CommunicationDefinitions.PACKET_SIZES[(CommunicationDefinitions.TYPE)recieve_buffer[0]];

                            recieve_state = RecieveState.Data;

                        }
                        else
                        {
                            recieve_state = RecieveState.Key;
                        }
                    }
                }
                else if (recieve_state == RecieveState.Data)
                {
                    if (client.Available < recieve_size)
                    {
                        break;
                    }

                    if (client.GetStream().Read(recieve_buffer, 1, recieve_size) == recieve_size)
                    {
                        CommunicationDefinitions.TYPE type = (CommunicationDefinitions.TYPE)recieve_buffer[0];
                        if (type == CommunicationDefinitions.TYPE.DATAAGGREGATOR_STATE)
                        {
                            StateData.dataaggregator_state.Deserialize(recieve_buffer);

                            parent.update_indicators();
                        }
                        else if (type == CommunicationDefinitions.TYPE.VISION)
                        {
                            StateData.vision_data.Deserialize(recieve_buffer);
                        }
                        else if (type == CommunicationDefinitions.TYPE.SENSOR_STATE)
                        {
                            StateData.sensor_state_data.Deserialize(recieve_buffer);
                        }
                        else if (type == CommunicationDefinitions.TYPE.VISION_IMAGE)
                        {
                            parent.vision_view.SetImage(recieve_buffer.Skip(1).ToArray());
                        }
                        else if (type == CommunicationDefinitions.TYPE.ROBOT_STATE)
                        {
                            StateData.robot_state_data.Deserialize(recieve_buffer);
                        }
                        else if (type == CommunicationDefinitions.TYPE.REALSENSE)
                        {
                            StateData.realsense_data.Deserialize(recieve_buffer);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Type");
                        }

                        recieve_state = RecieveState.Key;
                    }
                }

            }
        }

        public void send_buffered()
        {
            if (connection_state != ConnectionState.Connected)
            {
                return;
            }
            try
            {
                while (send_buffer.Count > 0)
                {

                    client.GetStream().Write(send_buffer.Last(), 0, send_buffer.Last().Length);
                    send_buffer.RemoveAt(send_buffer.Count - 1);
                }
            }
            catch
            {
                socket_disconnect();
            }
            
        }

        public void send_identifier()
        {
            byte[] identifier = new byte[128];
            identifier[0] = (byte)CommunicationDefinitions.TYPE.INDENTIFIER;
            identifier[1] = (byte)CommunicationDefinitions.IDENTIFIER.DASHBOARD;

            send_key(identifier);
        }

        Joystick stick = null;

        public void send_joystick()
        {
            if (stick == null || stick.Poll().IsFailure)
            {
                StateData.send_joystick_enabled = false;
                StateData.dashboard_state.enabled = false;
                StateData.mainwindow.Disable();
                MessageBox.Show("Joystick Disconnected");
                return;
            }
            stick.GetBufferedData();
            var state = stick.GetCurrentState();

            StateData.joystick_data.Load(state);

            send_key(StateData.joystick_data.Serialize());
        }

        public void socket_reconnect()
        {
            try
            {
                client = new TcpClient();
                client.SendBufferSize = 128;
                client.ReceiveBufferSize = 128000;
                client.ReceiveTimeout = 1000;
                client.SendTimeout = 1000;
                client.Connect(host, port);
                connection_state = ConnectionState.Connected;
                Console.WriteLine("Socket: Reconnect Succeeded");

            }
            catch
            {
                Console.WriteLine("Socket: Reconnect Failed");
                System.Threading.Thread.Sleep(500);
            }

            parent.update_indicators();
        }

        public void socket_disconnect()
        {
            Console.WriteLine("Socket: Disconnected");
            
            client.Close();
            client = null;
            connection_state = ConnectionState.Disconnected;
        }

        public void start_send_joystick()
        {
     

            try
            {
                stick = new SlimDX.DirectInput.Joystick(StateData.Input, StateData.joy_guid);
                stick.Properties.BufferSize = 128;
                if (stick.Acquire().IsFailure)
                {
                    throw new Exception("Joystick Aquire Failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                StateData.send_joystick_enabled = false;
                MessageBox.Show("Failed to Aquire Joystick");
                return;
            }

            StateData.send_joystick_enabled = true;
        }

        public void stop_send_joystick()
        {
            StateData.send_joystick_enabled = false;
        }
        

        public void send_key(byte[] buf)
        {
            List<byte> list = new List<byte>();
            list.AddRange(CommunicationDefinitions.key);
            list.AddRange(buf);

            byte[] data = list.ToArray();
            send(data);
        }
        public void send(byte[] buf)
        {
            send_buffer.Add(buf);
        }
      

        public void send_vision_properties()
        {
            byte[] data = StateData.properties.Serialize();

            send_key(data);
        }

        public void send_dashboard_state()
        {
            send_key(StateData.dashboard_state.Serialize());
        }

        public void send_vision_image()
        {
            byte[] data = new byte[128];
            data[0] = 12;
            data[1] = 0;
            send_key(data);
        }
    }
    
}
