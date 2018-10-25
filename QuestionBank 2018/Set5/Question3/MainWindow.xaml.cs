using DigitalLibrary;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
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
        Timer timer = new Timer();

        ADAM4150 adam;

        double tem, hum;
        bool flame, smoke;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            adam = new ADAM4150(new ComSettingModel() { DigitalQuantityCom = cmbADAM.SelectedItem.ToString() });

            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Interval = 100;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            adam.SetData();

            lblSmoke.Content = (smoke = adam.DI3) ? "报警" : "正常";
            lblFlame.Content = (smoke = adam.DI2) ? "报警" : "正常";

            lblTem.Content = (tem = 24.5) + "℃";
            lblHum.Content = (hum = 63.4) + "%RH";

            List<byte> datas = new List<byte>();

            datas.AddRange(new byte[] { 0xff, 0x01, 0x18 });
            datas.AddRange(BitConverter.GetBytes(tem));
            datas.AddRange(BitConverter.GetBytes(hum));
            datas.Add(0xff);

            byte[] byteDatas = datas.ToArray();

            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect("192.168.1.2", 9988);
            NetworkStream stream = tcpClient.GetStream();
            stream.Write(byteDatas.ToArray(), 0, byteDatas.Length);
        }

        public MainWindow()
        {
            InitializeComponent();

            cmbADAM.ItemsSource = cmbZigBee.ItemsSource = SerialPort.GetPortNames();

            cmbADAM.SelectedItem = "COM2";
            cmbZigBee.SelectedItem = "COM3";
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if ((string)lblState.Content == "未连接")
            {
                timer.Start();

                lblState.Content = "已连接";
                lblState.Background = new SolidColorBrush(Color.FromRgb(0x00, 0xff, 0x00));
            }
            else
            {
                timer.Stop();

                lblState.Content = "未连接";
                lblState.Background = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));

            }

        }
    }
}
