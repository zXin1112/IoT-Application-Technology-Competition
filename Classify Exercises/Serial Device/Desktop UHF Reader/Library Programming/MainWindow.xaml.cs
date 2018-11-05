using Srr1100U;
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
        SrrReader srrReader = null;

        public MainWindow()
        {
            InitializeComponent();

            srrReader = new SrrReader("COM4");
        }

        private void btnDevice_Click(object sender, RoutedEventArgs e)
        {
            if (btnDevice.Tag.ToString() == "0")
            {
                srrReader.ConnDevice();

                btnRead.IsEnabled = true;
                btnDevice.Content = "断开设备";
                btnDevice.Tag = "1";
            }
            else
            {
                srrReader.CloseDevice();

                btnRead.IsEnabled = false;
                btnDevice.Content = "连接设备";
                btnDevice.Tag = "0";
            }
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            srrReader.Read(new Action<string>((epc) =>
            {
                if (!lsbEPC.Items.Contains(epc))
                    Dispatcher.Invoke(() => { lsbEPC.Items.Add(epc); });
            }));
        }
    }
}
