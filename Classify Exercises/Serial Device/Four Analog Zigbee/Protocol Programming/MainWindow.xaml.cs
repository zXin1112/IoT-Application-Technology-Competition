using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Protocol_Programming
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort serialPort = null;

        public MainWindow()
        {
            InitializeComponent();

            serialPort = new SerialPort("COM1", 38400);
            serialPort.DataReceived += serialPort_DataReceived;
        }

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (serialPort.ReadByte() == 0xFE && serialPort.ReadByte() == 0x16)
            {
                byte[] datas = new byte[0x1a];
                datas[0] = 0xfe;
                datas[1] = 0x16;

                serialPort.Read(datas, 2, 0x1a - 2);

                double in1 = BitConverter.ToUInt16(datas, 18);
                double in2 = BitConverter.ToUInt16(datas, 20);
                double in3 = BitConverter.ToUInt16(datas, 22);

                this.Dispatcher.Invoke(() =>
                {
                    lblIN1.Content = in1;
                    lblIN2.Content = in2;
                    lblIN3.Content = in3;

                    lblLight.Content = ((in1 * 3300 / 1023 / 150 - 4) / 16 * 20000 + 0).ToString("f2");
                    lblTem.Content = ((in2 * 3300 / 1023 / 150 - 4) / 16 * 70 - 10).ToString("f2");
                    lblHum.Content = ((in3 * 3300 / 1023 / 150 - 4) / 16 * 50 + 50).ToString("f2");
                });

                Thread.Sleep(1000);
            }
        }

        private void btnGetData_Click(object sender, RoutedEventArgs e)
        {
            if ((string)btnGetData.Tag == "0")
            {
                serialPort.Open();
                btnGetData.Content = "停止监听";
                btnGetData.Tag = "1";
            }
            else
            {
                serialPort.Close();
                btnGetData.Content = "开始监听";
                btnGetData.Tag = "";
            }
        }


    }
}
