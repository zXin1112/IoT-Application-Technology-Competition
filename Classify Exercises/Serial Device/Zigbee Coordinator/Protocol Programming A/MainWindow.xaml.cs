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

namespace Protocol_Programming_A
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

            serialPort = new SerialPort();
            serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (serialPort.ReadByte() == 0xfe)
            {
                int dataLength = serialPort.ReadByte();

                while (serialPort.ReadByte() == 0x46 && serialPort.ReadByte() == 0x87)
                {
                    byte[] datas = new byte[dataLength + 1];
                    serialPort.Read(datas, 0, datas.Length);

                    dataLength = datas[5];
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
