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

            serialPort = new SerialPort("COM1", 38400);
            serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (serialPort.ReadByte() == 0xfe)
            {
                int dataLength = serialPort.ReadByte();

                while (serialPort.ReadByte() == 0x46 && serialPort.ReadByte() == 0x87)
                {
                    byte[] data = new byte[dataLength + 4];

                    data[0] = (byte)dataLength;
                    data[1] = 0x46;
                    data[2] = 0x87;

                    Thread.Sleep(300);

                    serialPort.Read(data, 3, data.Length - 4);

                    //if (data[data.Length - 1] == CheckDataZigbeeSensor(data.Take(data.Length - 1).ToArray()))
                    //    return;


                    switch (data[16])
                    {
                        case 0x01: LabelValue(lblTem, (double)BitConverter.ToInt16(data, 17) / 10 + " 度"); LabelValue(lblHum, (double)BitConverter.ToInt16(data, 19) / 10 + " %"); break;
                        case 0x11: LabelValue(lblBody, BitConverter.ToBoolean(data, 17) ? "有人" : "无人"); break;
                        case 0x21: LabelValue(lblLight, (double)BitConverter.ToUInt16(data, 17) / 100 + " V"); break;
                        case 0x22: LabelValue(lblCO, (double)BitConverter.ToUInt16(data, 17) / 100 + " V"); break;
                        case 0x23: LabelValue(lblGas, (double)BitConverter.ToUInt16(data, 17) / 100 + " V"); break;
                        case 0x24: LabelValue(lblFlame, (double)BitConverter.ToUInt16(data, 17) / 100 + " V"); break;
                    }




                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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

        private byte CheckDataZigbeeSensor(byte[] datas)
        {
            byte result = 0x00;

            foreach (byte data in datas)
                result = (byte)(result ^ data);

            return result;
        }

        private void LabelValue(Label label, object value)
        {
            Dispatcher.Invoke(() =>
            {
                label.Content = value;
            });
        }

        private void btnRelay1_Click(object sender, RoutedEventArgs e)
        {
            byte[] cmd = null;

            if ((string)btnRelay1.Tag == "0")
            {
                cmd = new byte[] { 0xFF, 0xF5, 0x05, 0x02, 0x01, 0x00, 0x00, 0x01, 0x00 };
                btnRelay1.Content = "关闭";
                btnRelay1.Tag = "1";
            }
            else
            {
                cmd = new byte[] { 0xFF, 0xF5, 0x05, 0x02, 0x01, 0x00, 0x00, 0x02, 0x00 };
                btnRelay1.Content = "打开";
                btnRelay1.Tag = "0";
            }

            if (!serialPort.IsOpen)
            {
                serialPort.Open();

                serialPort.Write(cmd, 0, cmd.Length);
                serialPort.DiscardOutBuffer();

                serialPort.Close();
            }
            else
            {
                serialPort.Write(cmd, 0, cmd.Length);
                serialPort.DiscardOutBuffer();
            }

        }

        private void btnRelay2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRelay3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
