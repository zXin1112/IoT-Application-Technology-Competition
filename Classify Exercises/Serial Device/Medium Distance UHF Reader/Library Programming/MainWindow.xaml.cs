using RFIDLibrary;
using System;
using System.Collections.Generic;
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

namespace Library_Programming
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        RFIDHelper rfidHelper = null;

        Thread thread = null;
        AutoResetEvent resetEvent = null;
        bool isStopThread = true;

        public MainWindow()
        {
            InitializeComponent();

            rfidHelper = new RFIDHelper("COM1");

            thread = new Thread(new ThreadStart(QueryEPC));
            thread.IsBackground = true;

            resetEvent = new AutoResetEvent(false);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            thread.Start();
        }

        private void QueryEPC()
        {
            while (true)
            {
                if (isStopThread)
                    resetEvent.WaitOne();

                if (rfidHelper.IsOpen)
                {
                    string epc = rfidHelper.ReadEpcSection();

                    Dispatcher.Invoke(new Action(() =>
                    {
                        if (!lsbEPC.Items.Contains(epc))
                            lsbEPC.Items.Add(epc);
                    }));
                }

                Thread.Sleep(500);
            }
        }

        private void btnConnectionDevice_Click(object sender, RoutedEventArgs e)
        {
            if (btnConnectionDevice.Tag.ToString() == "0")
            {
                string error = rfidHelper.Open();

                if (error != string.Empty)
                {
                    MessageBox.Show(error);
                    return;
                }

                btnConnectionDevice.Content = "关闭设备";
                btnConnectionDevice.Tag = "1";
            }
            else
            {
                rfidHelper.Close();

                btnConnectionDevice.Content = "打开设备";
                btnConnectionDevice.Tag = "0";
            }
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            //lsbEPC.ItemsSource = rfidHelper.Inventory();
            if (btnQuery.Tag.ToString() == "0")
            {
                isStopThread = false;
                resetEvent.Set();

                btnQuery.Content = "停止查询EPC";
                btnQuery.Tag = "1";
            }
            else
            {
                isStopThread = true;

                btnQuery.Content = "开始查询EPC";
                btnQuery.Tag = "0";
            }
        }

        private void btnWriteEPC_Click(object sender, RoutedEventArgs e)
        {
            if (lsbEPC.SelectedIndex == -1)
                return;

            string epc = lsbEPC.SelectedValue.ToString();

            byte[] byteEPC = rfidHelper.HexStringToByteArray(epc);

            string text = string.Join("", txbEPC.Text.Split(' '));
            int num;
            if ((num = text.Length % 4) != 0)
                for (int i = 0; i < (4 - num); i++)
                    text += "0";
            byte[] byteData = rfidHelper.HexStringToByteArray(text);


            rfidHelper.WriteSectionByte(byteEPC, 2, byteData, 1);
        }

        private void btnWriteUser_Click(object sender, RoutedEventArgs e)
        {
            if (lsbEPC.SelectedIndex == -1)
                return;

            string epc = lsbEPC.SelectedValue.ToString();

            byte[] byteEPC = rfidHelper.HexStringToByteArray(epc);

            string strData = txbUser.Text;
            int dataLength = strData.Length;

            byte[] data = Encoding.GetEncoding("GB2312").GetBytes(strData);

            //string text = string.Join("", txbUser.Text.Split(' '));
            //int num;
            //if ((num = text.Length % 4) != 0)
            //    for (int i = 0; i < (4 - num); i++)
            //        text += "0";
            //byte[] byteData = rfidHelper.HexStringToByteArray(text); 

            rfidHelper.WriteSectionByte(byteEPC, 0, data, 3);
        }

        private void btnReadData_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteData =rfidHelper.ReadUserSectionByte(0, 12);
            txbUser.Text = Encoding.GetEncoding("GB2312").GetString(byteData);
        }
    }
}
