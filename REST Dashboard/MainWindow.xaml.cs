﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SlimDX.DirectInput;

namespace REST_Dashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer send_joystick_timer = new DispatcherTimer();
        DispatcherTimer recieve_data_timer = new DispatcherTimer();


        Guid joy_guid;

        AsyncSocketClient socket;

        DashboardData dashboard_state;

        public MainWindow()
        {
            InitializeComponent();

            socket = new AsyncSocketClient(); 

            TimeSpan interval = new TimeSpan(0,0,0,0,50);
            send_joystick_timer.Interval = interval;

            TimeSpan recieve_interval = new TimeSpan(0, 0, 0, 0, 100);
            recieve_data_timer.Interval = recieve_interval;

            send_joystick_timer.Tick += new EventHandler(send_joystick_timer_elapsed);
            recieve_data_timer.Tick += new EventHandler(recieve_data_timer_elapsed);

            recieve_data_timer.Start();

            update_connected_indicator();

            dashboard_state =new  DashboardData();

        }
        private void recieve_data_timer_elapsed(object sender, EventArgs e)
        {
            update_connected_indicator();
            byte[] bytes = new byte[128];

            if(socket.recieve(ref bytes))
            {
                byte type = 8;
                if(type == 8)
                {
                    DashboardDataAggregatorState d = new DashboardDataAggregatorState();
                    d.Deserialize(bytes);

                    HeroConnectedIndicator.connected = d.hero_connected;
                }
            }

        }
        private void update_connected_indicator()
        {
            connected_indicator.label = "Connected";
            connected_indicator.connected = socket.connected();

            HeroConnectedIndicator.label = "HeroSerial";
        }

        private void send_joystick_timer_elapsed(object sender, EventArgs e)
        {
            send_joystick_data();
        }

        DirectInput Input = new DirectInput();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void list_joysticks()
        {
            List<SlimDX.DirectInput.Joystick> sticks = new List<SlimDX.DirectInput.Joystick>(); // Creates the list of joysticks connected to the computer via USB.

            ControllerSelect1.Items.Clear();
            foreach (DeviceInstance device in Input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                // Creates a joystick for each game device in USB Ports
                try
                {
                    ControllerSelect1.Items.Add(device);
                }
                catch (DirectInputException)
                {
                }
            }
        }

        byte joy2byte(int joy)
        {
            byte ret = (byte)((joy / 65535.0 * 2) * 127);
            return ret;
        }

        Joystick stick = null;

        private void send_joystick_data()
        {

            if (!socket.connected())
            {
                send_joystick_timer.Stop();
                return;
            }

            if (stick == null)
            {
                stick = new SlimDX.DirectInput.Joystick(Input, joy_guid);
                if (stick.Acquire().IsFailure)
                {
                    return;
                }
            }

            if (stick.Poll().IsFailure)
            {
                return;
            }

            var state = stick.GetCurrentState();
            

            JoystickData data = new DashboardJoystickData();

            data.button_a = state.GetButtons()[0];
            data.button_b = state.GetButtons()[1];
            data.button_x = state.GetButtons()[2];
            data.button_y = state.GetButtons()[3];

            data.button_lb = state.GetButtons()[4];
            data.button_rb = state.GetButtons()[5];
            data.button_select = state.GetButtons()[6];
            data.button_start = state.GetButtons()[7];
            data.button_lj = state.GetButtons()[8];
            data.button_rj = state.GetButtons()[9];

            data.lj_x = joy2byte(state.X);
            data.lj_y = joy2byte(state.Y);
            data.rj_x = joy2byte(state.RotationX);
            data.rj_y = joy2byte(state.RotationY);

            data.rt = joy2byte(state.Z);
            data.lt = joy2byte(state.Z);

            socket.send(data.Serialize());

        }

        private void send_dashboard_data()
        {
            socket.send(dashboard_state.Serialize());
        }

        private void enable_joystick_click(object sender, RoutedEventArgs e)
        {
            try
            {
                joy_guid = ((DeviceInstance)ControllerSelect1.SelectedItem).InstanceGuid;

                send_joystick_timer.Start();
            }
            catch
            {
                MessageBox.Show("Invalid joystick");
            }
            
        }

        private void ControllerSelect1_DropDownOpened(object sender, EventArgs e)
        {
            list_joysticks();
        }

        private void Enable_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.enabled = true;
            send_dashboard_data();
        }

        private void Disable_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.enabled = false;
            send_dashboard_data();
        }

        private void EStop_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.estop = true;
            dashboard_state.enabled = true;
            send_dashboard_data();
        }

        private void Teleop_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.robot_state = DashboardData.RobotStateEnum.Teleop;
        }

        private void Auton_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.robot_state = DashboardData.RobotStateEnum.Auton;
        }

        private void EStop_Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.estop = false;
            dashboard_state.enabled = false;
            send_dashboard_data();
        }


        private void disable_joystick_click(object sender, RoutedEventArgs e)
        {
            send_joystick_timer.Stop();
        }

        private void send_one_joystick_Click(object sender, RoutedEventArgs e)
        {
            joy_guid = ((DeviceInstance)ControllerSelect1.SelectedItem).InstanceGuid;
            send_joystick_data();
        }

        private void MainWindow1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Space)
            {
                e.Handled = true;
                dashboard_state.estop = true;
                send_dashboard_data();
            }
            
        }
    }
}