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
using ZigBeeLibrary;

namespace Library_Programming_B
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ZigBee fourAnalogZigbee = null;

        public MainWindow()
        {
            InitializeComponent();

            fourAnalogZigbee = new ZigBee(new ComSettingModel { ZigbeeCom = "COM1" });
        }

        private void btnGetData_Click(object sender, RoutedEventArgs e)
        {
            fourAnalogZigbee.GetSet();

            byte[] datas = fourAnalogZigbee.OriginalData;

            if (datas[1] != 0x16)
                return;

            byte[] sensorData = new byte[8];

            datas.Skip(18).Take(8).ToArray().CopyTo(sensorData, 0);

            int in1 = BitConverter.ToUInt16(sensorData, 0);
            int in2 = BitConverter.ToUInt16(sensorData, 2);
            int in3 = BitConverter.ToUInt16(sensorData, 4);

            lblIN1.Content = in1;
            lblIN2.Content = in2;
            lblIN3.Content = in3;

            double light = (((double)in1 * 3300 / 1023 / 150) - 4) / 16 * 20000 + 0;
            double tem = (((double)in2 * 3300 / 1023 / 150) - 4) / 16 * 70 - 10;
            double hum = (((double)in3 * 3300 / 1023 / 150) - 4) / 16 * 50 + 50;

            lblLight.Content = light.ToString("f2");
            lblTem.Content = tem.ToString("f2");
            lblHum.Content = hum.ToString("f2");
        }
    }
}
