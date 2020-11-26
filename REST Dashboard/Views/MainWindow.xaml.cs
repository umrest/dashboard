using System;
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
using comm;
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

        private CommunicationHandler communication;

        private Color lights_color;

        public MainWindow()
        {
            InitializeComponent();

            update_indicators();
            communication =  new CommunicationHandler();

            //communication.start_recieve_data();

            props.Add(StateData.properties);

            StateData.properties.PropertyChanged += Properties_PropertyChanged;

            StateData.mainwindow = this;

            
            VisionCaptureProperties.ItemsSource = props;

            list_joysticks();

            StateData.message.PropertyChanged += Message_PropertyChanged;
           
        }

        private void Message_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var buf = StateData.message.get_message();
            string str = Encoding.ASCII.GetString(buf.TakeWhile(x => x != 0).ToArray());
            var id = ((comm.CommunicationDefinitions.IDENTIFIER)StateData.message.get_identifier()).ToString();
            log_view.add_message(id, str);

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
         

        
        public void list_joysticks()
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
                HeroConnectedIndicator.connected = StateData.data_server.get_hero_connected();
                VisionConnectedIndicator.connected = StateData.data_server.get_vision_connected();
                TCPSerialConnectedIndicator.connected = StateData.data_server.get_tcpserial_connected();
                RealsenseConnectedIndicator.connected = StateData.data_server.get_realsense_connected();
                DashboardConnectedIndicator.connected = StateData.data_server.get_dashboard_connected();
                   
                Enable_Button.Background.Opacity = StateData.dashboard_state.enabled ? on : off;

                Disable_Button.Background.Opacity = !StateData.dashboard_state.enabled ? on : off;

                EStop_Button.Background.Opacity = StateData.dashboard_state.estop ? on : off;

                Auton_Button.Background.Opacity = StateData.dashboard_state.robot_state == DashboardDashboard.RobotStateEnum.Auton ? on : off;

                Teleop_Button.Background.Opacity = StateData.dashboard_state.robot_state == DashboardDashboard.RobotStateEnum.Teleop ? on : off;

                Test_Button.Background.Opacity = StateData.dashboard_state.robot_state == DashboardDashboard.RobotStateEnum.Test ? on : off;

            }

            ));
        }

        private void send_dashboard_data()
        {
            update_hardware();
            update_indicators();
            communication.send_dashboard_state();
        }

        private void ControllerSelect1_DropDownOpened(object sender, EventArgs e)
        {
            list_joysticks();
        }

        private void Enable_Button_Click(object sender, RoutedEventArgs e)
        {
            if(StateData.dashboard_state.estop == false)
            {
                StateData.dashboard_state.enabled = true;
                send_dashboard_data();
                if (StateData.dashboard_state.robot_state == DashboardDashboard.RobotStateEnum.Teleop)
                {
                    start_send_joystick();
                }
            }

            

            
            
        }

        public void Disable()
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                Disable_Button_Click(null, null);
            }));
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
            //Disable_Button_Click(null, null);

            StateData.dashboard_state.robot_state = DashboardDashboard.RobotStateEnum.Teleop;
            send_dashboard_data();
            start_send_joystick();
        }

        private void Auton_Button_Click(object sender, RoutedEventArgs e)
        {
            //Disable_Button_Click(null, null);

            StateData.dashboard_state.robot_state = DashboardDashboard.RobotStateEnum.Auton;
            send_dashboard_data();
            stop_send_joystick();
        }
        private void Test_Button_Click(object sender, RoutedEventArgs e)
        {
            //Disable_Button_Click(null, null);

            StateData.dashboard_state.robot_state = DashboardDashboard.RobotStateEnum.Test;
            send_dashboard_data();
            stop_send_joystick();

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
            bool was_running = StateData.send_joystick_enabled;
            
            if (ControllerSelect1.SelectedIndex != -1)
            {
                StateData.joy_guid = ((DeviceInstance)ControllerSelect1.SelectedItem).InstanceGuid;

               
            }
            if (was_running)
            {
                stop_send_joystick();
                start_send_joystick();
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

        private void stop_vision_streaming_button_Click(object sender, RoutedEventArgs e)
        {
            communication.stop_vision_streaming();
        }


        private void start_vision_streaming_button_Click(object sender, RoutedEventArgs e)
        {
            communication.start_vision_streaming();
        }

        private void stop_detection_button_Click(object sender, RoutedEventArgs e)
        {
            communication.stop_detection();
        }


        private void start_detection_button_Click(object sender, RoutedEventArgs e)
        {
            communication.start_detection();
        }

        private void send_depth_Click(object sender, RoutedEventArgs e)
        {
            communication.send_depth();
        }

        private void send_obstacle_Click(object sender, RoutedEventArgs e)
        {
            communication.send_obstacle();
        }

        private void angle_TextChanged(object sender, TextChangedEventArgs e)
        {
            update_hardware();
        }

        private void update_hardware()
        {
            lights_color.R = (byte)(StateData.dashboard_state.enabled ? 0 : 25);
            lights_color.G = (byte)(StateData.dashboard_state.enabled ? 25 : 0);
            lights_color.B = 0;

            if (communication != null)
            {
                int out_angle = 0;
                bool res1 = int.TryParse(angle.Text, out out_angle);
                
                if (res1)
                {

                    communication.send_hardware(out_angle, lights_color.R, lights_color.G, lights_color.B);
                }


            }

        }

        private void ExcavateLocation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SieveAlign_Click(object sender, RoutedEventArgs e)
        {
            field_view.target_rover_position.X = 0;
            field_view.target_rover_position.Y = 15;
            field_view.set_target_rover_position();
        }

        private void VisionPropertiesSubmit_Click(object sender, RoutedEventArgs e)
        {
            StateData.properties.set_exposure(uint.Parse(exposure.Text));
            StateData.properties.set_gain(uint.Parse(gain.Text));
            communication.send_vision_properties();
        }

        private void b_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            update_hardware();
        }

        private void g_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            update_hardware();

        }

        private void r_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            update_hardware();
        }
    }
}
