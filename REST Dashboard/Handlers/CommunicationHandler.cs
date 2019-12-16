﻿using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace REST_Dashboard.Handlers
{
    public class CommunicationHandler
    {

        ThreadStart recieve_data_ts;
        ThreadStart send_joystick_ts;

        Thread recieve_data_thread;
        Thread send_joystick_thread;


        AsyncSocketClient socket;

        DirectInput Input = new DirectInput();

        MainWindow parent = null;

        public CommunicationHandler(MainWindow parent_in) {
            socket = new AsyncSocketClient();

            send_joystick_ts = new ThreadStart(send_joystick_data);

            recieve_data_ts = new ThreadStart(recieve_data);
            recieve_data_thread = new Thread(recieve_data_ts);

            parent = parent_in;
            
        }

        public void start_recieve_data()
        {
            recieve_data_thread.Start();
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

        private void recieve_data()
        {
            int delay = 100;


            while (true)
            {
                parent.update_indicators();
                System.Threading.Thread.Sleep(delay);
                try
                {
                    // slow down the timer if we can't connect
                    if (!socket.connected())
                    {

                        delay = 2500;
                        socket.connect();
                        continue;
                    }
                    else
                    {

                        delay = 100;
                    }

                    List<byte[]> msgs = new List<byte[]>();
                    if (socket.recieve(msgs))
                    {
                        foreach (byte[] bytes in msgs)
                        {

                            byte type = bytes[0];
                            if (type == 8)
                            {
                                StateData.dataaggregator_state.Deserialize(bytes);

                                parent.update_indicators();
                            }
                            else if (type == 2)
                            {
                                StateData.vision_data.Deserialize(bytes);
                            }
                            else if (type == 10)
                            {
                                StateData.robot_state_data.Deserialize(bytes);
                            }
                            else if (type == 13)
                            {
                                parent.vision_view.SetImage(bytes.Skip(1).ToArray());
                            }
                            else
                            {
                                Console.WriteLine("Invalid Type");
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Exception in Recieve Data");
                    delay = 1000;
                }

            }


        }

        public void send_joystick_data()
        {
            Joystick stick = null;

            try
            {
                stick = new SlimDX.DirectInput.Joystick(Input, StateData.joy_guid);
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

                socket.send(StateData.joystick_data.Serialize());

                System.Threading.Thread.Sleep(50);
            }
        }

        public void send_dashboard_state()
        {
            socket.send(StateData.dashboard_state.Serialize());
        }

        public bool connected()
        {
            return socket.connected();
        }

        public List<DeviceInstance> get_joysticks()
        {
            return Input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly).ToList();
        }

        public void send_vision_image()
        {
            byte[] data = new byte[128];
            data[0] = 12;
            data[1] = 0;
            socket.send(data);
        }
    }
}