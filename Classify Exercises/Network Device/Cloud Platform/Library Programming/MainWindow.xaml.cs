using NLECloudSDK;
using NLECloudSDK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Library_Programming
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int projectId = 20339;
        int deviceId = 21742;
        string token;

        NLECloudAPI api = null;

        public MainWindow()
        {
            InitializeComponent();

            api = new NLECloudAPI("http://api.nlecloud.com");
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            AccountLoginDTO loginDTO = new AccountLoginDTO() { Account = "ytvciot", Password = "123456" };

            ResultMsg<AccountLoginResultDTO> result = api.UserLogin(loginDTO);
            token = result.ResultObj.AccessToken;
        }

        private void btnSensorData_Click(object sender, RoutedEventArgs e)
        {
            ResultMsg<IEnumerable<DeviceSensorDataDTO>> dataDTO = api.GetDevicesDatas("21742", token);

            foreach (SensorDataDTO dto in dataDTO.ResultObj.First().Datas)
            {
                switch (dto.ApiTag)
                {
                    case "z_wind_speed": lblIN1.Content = dto.Value + " m/s"; break;
                    case "z_pressure": lblIN2.Content = dto.Value + " kpa"; break;
                    case "z_co2": lblIN3.Content = dto.Value + " ppm"; break;
                    case "z_air_quality": lblIN4.Content = dto.Value; break;
                }
            }
        }

        private void btnFan01_Click(object sender, RoutedEventArgs e)
        {
            if (btnFan01.Tag.ToString() == "0")
            {
                Result result = api.Cmds(deviceId, "z_Fan_01", 1, token);

                if (result.IsSuccess())
                {
                    btnFan01.Content = "关";
                    btnFan01.Tag = "1";
                }
                else
                    lblMessage.Content = result.Msg;

            }
            else
            {
                Result result = api.Cmds(deviceId, "z_Fan_01", 0, token);

                if (result.IsSuccess())
                {
                    btnFan01.Content = "开";
                    btnFan01.Tag = "0";
                }
                else
                    lblMessage.Content = result.Msg;
            }

        }

        private void btnFan02_Click(object sender, RoutedEventArgs e)
        {
            if (btnFan02.Tag.ToString() == "0")
            {
                Result result = api.Cmds(deviceId, "z_Fan_02", 1, token);

                if (result.IsSuccess())
                {
                    btnFan02.Content = "关";
                    btnFan02.Tag = "1";
                }
                else
                    lblMessage.Content = result.Msg;

            }
            else
            {
                Result result = api.Cmds(deviceId, "z_Fan_02", 0, token);

                if (result.IsSuccess())
                {
                    btnFan02.Content = "开";
                    btnFan02.Tag = "0";
                }
                else
                    lblMessage.Content = result.Msg;
            }
        }

        private void btnLamp01_Click(object sender, RoutedEventArgs e)
        {
            if (btnLamp01.Tag.ToString() == "0")
            {
                Result result = api.Cmds(deviceId, "z_Lamp_01", 1, token);

                if (result.IsSuccess())
                {
                    btnLamp01.Content = "关";
                    btnLamp01.Tag = "1";
                }
                else
                    lblMessage.Content = result.Msg;

            }
            else
            {
                Result result = api.Cmds(deviceId, "z_Lamp_01", 0, token);

                if (result.IsSuccess())
                {
                    btnLamp01.Content = "开";
                    btnLamp01.Tag = "0";
                }
                else
                    lblMessage.Content = result.Msg;
            }
        }

        private void btnLamp02_Click(object sender, RoutedEventArgs e)
        {
            if (btnLamp02.Tag.ToString() == "0")
            {
                Result result = api.Cmds(deviceId, "z_Lamp_02", 1, token);

                if (result.IsSuccess())
                {
                    btnLamp02.Content = "关";
                    btnLamp02.Tag = "1";
                }
                else
                    lblMessage.Content = result.Msg;

            }
            else
            {
                Result result = api.Cmds(deviceId, "z_Lamp_02", 0, token);

                if (result.IsSuccess())
                {
                    btnLamp02.Content = "开";
                    btnLamp02.Tag = "0";
                }
                else
                    lblMessage.Content = result.Msg;
            }
        }
    }
}
