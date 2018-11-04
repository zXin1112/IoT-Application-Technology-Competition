using MWRDemoDll;
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
                mifareRFEYE.ConnDevice();

                btnDevice.Content = "断开设备";
                btnDevice.Tag = "1";
            }
            else
            {
                mifareRFEYE.CloseDevice();

                btnDevice.Content = "连接设备";
                btnDevice.Tag = "0";
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            mifareRFEYE.Search();
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            mifareRFEYE.AuthCardPwd(null, (CardDataKind)sdrCardKind.Value);
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            txbRead.Text = mifareRFEYE.ReadString();
        }

        private void btnWrite_Click(object sender, RoutedEventArgs e)
        {
            mifareRFEYE.WriteString((CardDataKind)sdrCardKind.Value, txbWrite.Text, (int)sdrKindNum.Value, Encoding.GetEncoding("GB2312"));
        }
    }
}
