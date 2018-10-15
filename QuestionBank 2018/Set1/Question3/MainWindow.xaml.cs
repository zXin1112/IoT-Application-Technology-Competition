using NLECloudSDK;
using NLECloudSDK.Model;
using System;
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

namespace Question3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer timer;

        NLECloudAPI api;
        private string token;

        public MainWindow()
        {
            InitializeComponent();

            timer = new Timer();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Interval = 1000;
            timer.Start();

            api = new NLECloudAPI("http://api.nlecloud.com");
            Login();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MonitorCloud();
        }

        private void Login()
        {
            AccountLoginDTO dto = new AccountLoginDTO() { Account = "ytvciot", Password = "123456" };
            ResultMsg<AccountLoginResultDTO> result = api.UserLogin(dto);
            if (result.IsSuccess())
                token = result.ResultObj.AccessToken;
            else
                MessageBox.Show(result.ToString());
        }

        private void MonitorCloud()
        {
            ResultMsg<SensorBaseInfoDTO> result = api.GetSensorInfo(908, "z_pressure", token);
            this.Dispatcher.Invoke(new Action(() => { lblAtm.Content = result.ResultObj.Value + "kpa"; }));

            result = api.GetSensorInfo(908, "z_wind_speed", token);
            this.Dispatcher.Invoke(new Action(() => { lblWind.Content = result.ResultObj.Value + "m/s"; }));

            double windNum, limitNum;

            this.Dispatcher.Invoke(new Action(() =>
            {
                if (double.TryParse(result.ResultObj.Value.ToString(), out windNum) && double.TryParse(txbThr.Text, out limitNum))
                {
                    if (windNum < limitNum)
                    {
                        api.Cmds(908, "alarm", 0, token);
                    }
                    else
                    {
                        api.Cmds(908, "alarm", 1, token);
                    }
                }
            }));

        }
    }
}
