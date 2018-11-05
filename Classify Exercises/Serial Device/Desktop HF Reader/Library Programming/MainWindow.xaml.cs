using MWRDemoDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        MifareRFEYE mifareRFEYE = MifareRFEYE.Instance;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDevice_Click(object sender, RoutedEventArgs e)
        {
            if (btnDevice.Tag.ToString() == "0")
            {
                ResultMessage message = mifareRFEYE.ConnDevice();

                if (message.Result == Result.Success)
                {
                    btnDevice.Content = "断开设备";
                    btnDevice.Tag = "1";
                }
                else
                    lblMessage.Content = message.OutInfo;

            }
            else
            {
                ResultMessage message = mifareRFEYE.CloseDevice();

                if (message.Result == Result.Success)
                {
                    btnDevice.Content = "连接设备";
                    btnDevice.Tag = "0";
                }
                else
                    lblMessage.Content = message.OutInfo;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ResultMessage message = mifareRFEYE.Search();

            if (message.Result == Result.Success)
            {
                lblMessage.Content = "寻卡成功 " + message.Model;
            }
            else
                lblMessage.Content = message.OutInfo;
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            ResultMessage message = mifareRFEYE.AuthCardPwd(null, (CardDataKind)sdrCardKind.Value);

            lblMessage.Content = message.OutInfo;
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            ResultMessage message = mifareRFEYE.Read((CardDataKind)sdrCardKind.Value, (int)sdrKindNum.Value);

            if (message.Result == Result.Success)
            {
                txbRead.Text = Encoding.GetEncoding("GB2312").GetString((byte[])message.Model).Split('\0')[0].Trim();
            }
            else
                lblMessage.Content = message.OutInfo;
        }

        private void btnWrite_Click(object sender, RoutedEventArgs e)
        {
            string text = txbWrite.Text.Trim();

            int count = 0;

            Regex regex = new Regex(@"^[\u4e00-\u9fa5]{0,}$");

            foreach (char a in text)
            {
                if (regex.IsMatch(a.ToString()))
                    count += 2;
                else
                    count += 1;
            }

            while (count < 16)
            {
                text += " ";
                count++;
            }

            byte[] data = Encoding.GetEncoding("GB2312").GetBytes(text);

            ResultMessage message = mifareRFEYE.Write((CardDataKind)sdrCardKind.Value, data, (int)sdrKindNum.Value);

            lblMessage.Content = message.OutInfo;
        }
    }
}
