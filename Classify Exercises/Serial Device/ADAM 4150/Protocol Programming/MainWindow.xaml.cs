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

        byte[] openDO0 = CRC_16.GetCRC(new byte[] { 0x01, 0x05, 0x00, 0x10, 0xFF, 0x00 }, true);
        byte[] closeDO0 = CRC_16.GetCRC(new byte[] { 0x01, 0x05, 0x00, 0x10, 0x00, 0x00 }, true);

        byte[] openDO1 = CRC_16.GetCRC(new byte[] { 0x01, 0x05, 0x00, 0x11, 0xFF, 0x00 }, true);
        byte[] closeDO1 = CRC_16.GetCRC(new byte[] { 0x01, 0x05, 0x00, 0x11, 0x00, 0x00 }, true);

        byte[] openDO2 = CRC_16.GetCRC(new byte[] { 0x01, 0x05, 0x00, 0x12, 0xFF, 0x00 }, true);
        byte[] closeDO2 = CRC_16.GetCRC(new byte[] { 0x01, 0x05, 0x00, 0x12, 0x00, 0x00 }, true);

        byte[] requestData = CRC_16.GetCRC(new byte[] { 0x01, 0x01, 0x00, 0x00, 0x00, 0x07 }, true);

        public MainWindow()
        {
            InitializeComponent();

            serialPort = new SerialPort("COM2", 9600);
        }

        private void btnReceive_Click(object sender, RoutedEventArgs e)
        {
            serialPort.Open();
            serialPort.Write(requestData, 0, requestData.Length);

            Thread.Sleep(1000);

            int count = serialPort.BytesToRead;
            byte[] datas = new byte[count];

            serialPort.Read(datas, 0, count);

            if (!CRC_16.CheckCRC(datas))
                return;

            string data = Convert.ToString(datas[3], 2).PadLeft(7, '0');

            char[] charDatas = data.Reverse().ToArray();
            lblDI0.Content = charDatas[0];
            lblDI1.Content = charDatas[1];
            lblDI2.Content = charDatas[2];
            lblDI3.Content = charDatas[3];

            serialPort.Close();
        }

        private void btnDO0_Click(object sender, RoutedEventArgs e)
        {
            serialPort.Open();
            if ((string)btnDO0.Content == "打开")
            {
                serialPort.Write(openDO0, 0, openDO0.Length);
                btnDO0.Content = "关闭";
            }
            else
            {
                serialPort.Write(closeDO0, 0, closeDO0.Length);
                btnDO0.Content = "打开";
            }

            serialPort.Close();
        }

        private void btnDO1_Click(object sender, RoutedEventArgs e)
        {
            serialPort.Open();
            if ((string)btnDO1.Content == "打开")
            {
                serialPort.Write(openDO1, 0, openDO1.Length);
                btnDO1.Content = "关闭";
            }
            else
            {
                serialPort.Write(closeDO1, 0, closeDO1.Length);
                btnDO1.Content = "打开";
            }

            serialPort.Close();
        }

        private void btnDO2_Click(object sender, RoutedEventArgs e)
        {
            serialPort.Open();
            if ((string)btnDO2.Content == "打开")
            {
                serialPort.Write(openDO2, 0, openDO2.Length);
                btnDO2.Content = "关闭";
            }
            else
            {
                serialPort.Write(closeDO2, 0, closeDO2.Length);
                btnDO2.Content = "打开";
            }

            serialPort.Close();
        }
    }
}
