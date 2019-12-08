using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using REST_Dashboard.Utils;
using SlimDX.DirectInput;
using Timer = System.Timers.Timer;

namespace REST_Dashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ThreadStart recieve_data_ts;
        ThreadStart send_joystick_ts;

        Thread recieve_data_thread;
        Thread send_joystick_thread;

        Guid joy_guid;

        AsyncSocketClient socket;

        DashboardData dashboard_state;

        GlobalHotkey space_hotkey;

        bool send_joystick_enabled = false;

        public MainWindow()
        {
            InitializeComponent();

            socket = new AsyncSocketClient();


            send_joystick_ts = new ThreadStart(send_joystick_data);


            update_connected_indicator();

            dashboard_state = new DashboardData();

            update_indicators();
            

            recieve_data_ts = new ThreadStart(recieve_data);
            recieve_data_thread = new Thread(recieve_data_ts);
            recieve_data_thread.Start();
        }


        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // Space EStop hotkey
            /*
            space_hotkey = new GlobalHotkey(this, delegate ()
            {
                EStop_Button_Click(null, null);
            });
            */
        }
         private void recieve_data()
        {
            int delay = 100;


            while (true)
            {
                update_connected_indicator();
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
                                DashboardDataAggregatorState d = new DashboardDataAggregatorState();
                                d.Deserialize(bytes);

                                HeroConnectedIndicator.Dispatcher.BeginInvoke((Action)(() => HeroConnectedIndicator.connected = d.hero_connected));
                                VisionConnectedIndicator.Dispatcher.BeginInvoke((Action)(() => VisionConnectedIndicator.connected = d.vision_connected));
                            }
                            else if (type == 2)
                            {
                                vision_state_view.Dispatcher.BeginInvoke((Action)(() => vision_state_view.dashboardVisionData.Deserialize(bytes)));
                            }
                            else if (type == 10)
                            {
                                robot_state_view.Dispatcher.BeginInvoke((Action)(() => robot_state_view.robot_state.Deserialize(bytes)));
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
        private void update_connected_indicator()
        {
            connected_indicator.Dispatcher.BeginInvoke((Action)(() => connected_indicator.connected = socket.connected()));
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

        

        private void start_send_joystick()
        {
            send_joystick_enabled = true;
            if (send_joystick_thread == null || !send_joystick_thread.IsAlive)
            {
                send_joystick_thread = new Thread(send_joystick_ts);
                send_joystick_thread.Start();
            }
        }

        private void stop_send_joystick()
        {
            send_joystick_enabled = false;
        }
        private void send_joystick_data()
        {
            Joystick stick = null;

                try
            {
                stick = new SlimDX.DirectInput.Joystick(Input, joy_guid);
                stick.Properties.BufferSize = 128;
                if (stick.Acquire().IsFailure)
                {

                    throw new Exception("Joystick Aquire Failed");
                }
            }
            catch
            {
                send_joystick_enabled = false;
                MessageBox.Show("Failed to Aquire Joystick");
                return;
            }
                
            while (send_joystick_enabled)
            {
                
                if (stick.Poll().IsFailure)
                {
                    return;
                }
                stick.GetBufferedData();
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

                System.Threading.Thread.Sleep(50);
            }
        }

        private void update_indicators()
        {
            double on = 1;
            double off = .25;
            Enable_Button.Background.Opacity = dashboard_state.enabled ? on : off;

            Disable_Button.Background.Opacity = !dashboard_state.enabled ? on : off;

            EStop_Button.Background.Opacity = dashboard_state.estop ? on : off;

            Auton_Button.Background.Opacity = dashboard_state.robot_state == DashboardData.RobotStateEnum.Auton ? on : off;

            Teleop_Button.Background.Opacity = dashboard_state.robot_state == DashboardData.RobotStateEnum.Teleop ? on : off;

            Test_Button.Background.Opacity = dashboard_state.robot_state == DashboardData.RobotStateEnum.Test ? on : off;
        }

        private void send_dashboard_data()
        {
            update_indicators();
            socket.send(dashboard_state.Serialize());

        }

        private void ControllerSelect1_DropDownOpened(object sender, EventArgs e)
        {
            list_joysticks();
        }

        private void Enable_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.enabled = true;
            send_dashboard_data();
            if(dashboard_state.robot_state == DashboardData.RobotStateEnum.Teleop)
            {
                start_send_joystick();
            }           
            

        }

        private void Disable_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.enabled = false;
            send_dashboard_data();

            stop_send_joystick();
        }

        private void EStop_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.estop = true;
            dashboard_state.enabled = false;
            send_dashboard_data();
        }

        private void Teleop_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.robot_state = DashboardData.RobotStateEnum.Teleop;
            send_dashboard_data();

            Disable_Button_Click(null, null);
        }

        private void Auton_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.robot_state = DashboardData.RobotStateEnum.Auton;
            send_dashboard_data();

            Disable_Button_Click(null, null);
        }
        private void Test_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.robot_state = DashboardData.RobotStateEnum.Test;
            send_dashboard_data();

            Disable_Button_Click(null, null);
        }


        private void EStop_Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            dashboard_state.estop = false;
            dashboard_state.enabled = false;
            send_dashboard_data();
        }


        private void disable_joystick_click(object sender, RoutedEventArgs e)
        {
            stop_send_joystick();
        }

        private void send_one_joystick_Click(object sender, RoutedEventArgs e)
        {
            joy_guid = ((DeviceInstance)ControllerSelect1.SelectedItem).InstanceGuid;
            send_joystick_data();
        }

        private void ControllerSelect1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ControllerSelect1.SelectedIndex != -1)
            {
                joy_guid = ((DeviceInstance)ControllerSelect1.SelectedItem).InstanceGuid;

                bool was_running = send_joystick_thread != null && send_joystick_thread.IsAlive;
                if (was_running)
                {
                    stop_send_joystick();
                    start_send_joystick();
                }
            }
            else
            {
                stop_send_joystick();
            }
        }

    }
}
