﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using REST_Dashboard.Handlers;
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
        



        ObservableCollection<DashboardVisionCaptureProperties> props = new ObservableCollection<DashboardVisionCaptureProperties>();

        private CommunicationHandlerNew communication;

        public MainWindow()
        {
            InitializeComponent();

            update_indicators();
            communication =  new CommunicationHandlerNew(this);

            //communication.start_recieve_data();

            props.Add(StateData.properties);

            StateData.properties.PropertyChanged += Properties_PropertyChanged;

            
            VisionCaptureProperties.ItemsSource = props;

            list_joysticks();
           
        }

        private void Properties_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            communication.send_vision_properties();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            /*base.OnSourceInitialized(e);
            // Space EStop hotkey
            
            space_hotkey = new GlobalHotkey(this, delegate ()
            {
                EStop_Button_Click(null, null);
            });
            */
        }
         

        
        private void list_joysticks()
        {
            List<DeviceInstance> sticks = StateData.get_joysticks(); // Creates the list of joysticks connected to the computer via USB.


            ControllerSelect1.Items.Clear();
            foreach (DeviceInstance device in sticks)
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

            ControllerSelect1.SelectedIndex = 0;
        }

        private void start_send_joystick()
        {
            communication.start_send_joystick();
        }

        private void stop_send_joystick()
        {
            communication.stop_send_joystick();
        }
        

        public void update_indicators()
        {
            double on = 1;
            double off = .25;

            Dispatcher.BeginInvoke((Action)(() =>
            {
                connected_indicator.connected = communication.connected();
                HeroConnectedIndicator.connected = StateData.dataaggregator_state.hero_connected;
                VisionConnectedIndicator.connected = StateData.dataaggregator_state.vision_connected;

                Enable_Button.Background.Opacity = StateData.dashboard_state.enabled ? on : off;

                Disable_Button.Background.Opacity = !StateData.dashboard_state.enabled ? on : off;

                EStop_Button.Background.Opacity = StateData.dashboard_state.estop ? on : off;

                Auton_Button.Background.Opacity = StateData.dashboard_state.robot_state == DashboardData.RobotStateEnum.Auton ? on : off;

                Teleop_Button.Background.Opacity = StateData.dashboard_state.robot_state == DashboardData.RobotStateEnum.Teleop ? on : off;

                Test_Button.Background.Opacity = StateData.dashboard_state.robot_state == DashboardData.RobotStateEnum.Test ? on : off;

            }

            ));
        }

        private void send_dashboard_data()
        {
            update_indicators();
            communication.send_dashboard_state();
        }

        private void ControllerSelect1_DropDownOpened(object sender, EventArgs e)
        {
            list_joysticks();
        }

        private void Enable_Button_Click(object sender, RoutedEventArgs e)
        {
            StateData.dashboard_state.enabled = true;
            send_dashboard_data();
            if(StateData.dashboard_state.robot_state == DashboardData.RobotStateEnum.Teleop)
            {
                start_send_joystick();
            }
        }

        private void Disable_Button_Click(object sender, RoutedEventArgs e)
        {
            StateData.dashboard_state.enabled = false;
            send_dashboard_data();

            stop_send_joystick();
        }

        private void EStop_Button_Click(object sender, RoutedEventArgs e)
        {
            StateData.dashboard_state.estop = true;
            StateData.dashboard_state.enabled = false;
            send_dashboard_data();
        }

        private void Teleop_Button_Click(object sender, RoutedEventArgs e)
        {
            Disable_Button_Click(null, null);

            StateData.dashboard_state.robot_state = DashboardData.RobotStateEnum.Teleop;
            send_dashboard_data();
        }

        private void Auton_Button_Click(object sender, RoutedEventArgs e)
        {
            Disable_Button_Click(null, null);

            StateData.dashboard_state.robot_state = DashboardData.RobotStateEnum.Auton;
            send_dashboard_data();
        }
        private void Test_Button_Click(object sender, RoutedEventArgs e)
        {
            Disable_Button_Click(null, null);

            StateData.dashboard_state.robot_state = DashboardData.RobotStateEnum.Test;
            send_dashboard_data();

            
        }


        private void EStop_Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            StateData.dashboard_state.estop = false;
            StateData.dashboard_state.enabled = false;
            send_dashboard_data();
        }


        private void disable_joystick_click(object sender, RoutedEventArgs e)
        {
            stop_send_joystick();
        }

        private void send_one_joystick_Click(object sender, RoutedEventArgs e)
        {
            //communication.send_joystick_data();
        }

        private void ControllerSelect1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ControllerSelect1.SelectedIndex != -1)
            {
                StateData.joy_guid = ((DeviceInstance)ControllerSelect1.SelectedItem).InstanceGuid;

                bool was_running = StateData.send_joystick_enabled;
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


        private void MainWindow1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                EStop_Button_Click(sender, e);
                e.Handled = true;
            }
        }

        private void send_vision_image_button_Click(object sender, RoutedEventArgs e)
        {
            communication.send_vision_image();
        }
    }
}
