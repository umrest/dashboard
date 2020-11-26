﻿
using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using comm;

namespace REST_Dashboard.Handlers
{


    public class CommunicationHandler : DashboardClient
    {
        ThreadStart connection_ts;
        Thread connection_thread;

        public CommunicationHandler()
        {
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
            if (StateData.send_joystick_enabled && (now - last_joystick_send).TotalMilliseconds > 1)
            {
                send_joystick();
                last_joystick_send = now;
            }

            if ((now - last_heartbeat_send).TotalMilliseconds > 500)
            {
                send_identifier();
                last_heartbeat_send = now;
            }

            if ((now - last_state_send).TotalMilliseconds > 250)
            {
                send_dashboard_state();
                last_state_send = now;
            }

            RESTPacket[] messages = get_messages();
            foreach(RESTPacket message in messages)
            {
                if(message.type() == CommunicationDefinitions.TYPE.DATA_SERVER)
                {
                    StateData.data_server.Deserialize(message.Serialize());
                    StateData.mainwindow.update_indicators();
                }
                else if(message.type() == CommunicationDefinitions.TYPE.VISION_IMAGE)
                {
                    StateData.mainwindow.vision_view.SetImage(((Vision_Image)message).get_image());
                }
                else if (message.type() == CommunicationDefinitions.TYPE.VISION)
                {
                    StateData.vision.Deserialize(message.Serialize());
                }
                else if (message.type() == CommunicationDefinitions.TYPE.REALSENSE)
                {
                    StateData.realsense.Deserialize(message.Serialize());
                }
                else if (message.type() == CommunicationDefinitions.TYPE.SENSOR_STATE)
                {
                    StateData.sensor_state.Deserialize(message.Serialize());
                }
                else if (message.type() == CommunicationDefinitions.TYPE.ROBOT_STATE)
                {
                    StateData.robot_state.Deserialize(message.Serialize());
                }
                else if (message.type() == CommunicationDefinitions.TYPE.SLAM_STATE)
                {
                    StateData.navigation_state.Deserialize(message.Serialize());
                }
                else if (message.type() == CommunicationDefinitions.TYPE.NAVIGATION_OBSTACLES)
                {
                    StateData.navigation_obstacles.Deserialize(message.Serialize());
                }
                else if (message.type() == CommunicationDefinitions.TYPE.NAVIGATION_PATH)
                {
                    StateData.navigation_path.Deserialize(message.Serialize());
                }
                else if (message.type() == CommunicationDefinitions.TYPE.DEBUG_MESSAGE)
                {
                    StateData.message.Deserialize(message.Serialize());
                }
            }
            System.Threading.Thread.Sleep(20);
        }

        SlimDX.DirectInput.Joystick stick = null;

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

            send_message(StateData.joystick_data);
        }

        public void send_vision_properties()
        {
            send_message(StateData.properties);
        }

        public void send_dashboard_state()
        {
            send_message(StateData.dashboard_state);
        }

        public void send_vision_image()
        {
            Vision_Command message = new Vision_Command();
            message.set_command(5);
            send_message(message);
        }

        public void start_vision_streaming()
        {
            Vision_Command message = new Vision_Command();
            message.set_command(6);
            send_message(message);
        }

        public void stop_vision_streaming()
        {
            Vision_Command message = new Vision_Command();
            message.set_command(7);
            send_message(message);
        }

        public void start_detection()
        {
            Vision_Command message = new Vision_Command();
            message.set_command(8);
            send_message(message);
        }

        public void stop_detection()
        {
            Vision_Command message = new Vision_Command();
            message.set_command(9);
            send_message(message);
        }

        public void send_depth()
        {
            Realsense_Command message = new Realsense_Command();
            message.set_command(9);
            send_message(message);
        }
        public void send_obstacle()
        {
            comm.Realsense_Command message = new Realsense_Command();
            message.set_command(10);

            send_message(message);
        }

        internal void send_hardware(int angle, byte r, byte g, byte b)
        {
            comm.Hardware message = new Hardware();
            message.set_angle((byte)angle);
            message.set_r(r);
            message.set_g(g);
            message.set_b(b);

            send_message(message);
        }
    }

}
