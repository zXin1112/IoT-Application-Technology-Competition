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

namespace Protocol_Programming
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

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            byte[] head = new byte[] { 0xAA, 0x01, 0xBB, 0x51, 0x54 };
            byte[] type = new byte[] { 0x01, 0x00, 0x64, 0x00, 0x64 }; 
            byte[] datas = Encoding.Default.GetBytes(txbData.Text);

            byte[] sendData = new byte[datas.Length + 12];

            byte check = 0;

            foreach (byte data in type)
                check += data;

            foreach (byte data in datas)
                check += data;

            head.CopyTo(sendData, 0);
            sendData[5] = check;
            type.CopyTo(sendData, 6);
            datas.CopyTo(sendData, 11);
            sendData[sendData.Length - 1] = 0xFF;

            using (SerialPort serialPort = new SerialPort("COM5", 9600))
            {
                serialPort.Open();
                serialPort.Write(sendData, 0, sendData.Length);
            }
        }
    }
}
