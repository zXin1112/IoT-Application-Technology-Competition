using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace Question3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbPort.ItemsSource = SerialPort.GetPortNames();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            byte[] head = new byte[] { 0xAA, 0x01, 0xBB, 0x51, 0x54 };
            byte[] type = new byte[] { 1, 2, 100, 0, 100 };
            byte[] datas = Encoding.Default.GetBytes(txbText.Text);

            byte[] sendData = new byte[datas.Length + 12];

            head.CopyTo(sendData, 0);
            head[5] = NewMethod(datas);
            type.CopyTo(sendData, 6);
            datas.CopyTo(sendData, datas.Length + 6);
            datas[datas.Length - 1] = 0xFF;

            using (SerialPort serialPort = new SerialPort(cmbPort.SelectedItem.ToString(), 9600))
            {
                serialPort.Open();
                serialPort.Write(datas, 0, datas.Length);
            }
        }

        private static byte NewMethod(byte[] datas)
        {
            int result = 0;
            foreach (byte data in datas)
                result += data;

            return (byte)(result &= 0xFF);
        }
    }
}
