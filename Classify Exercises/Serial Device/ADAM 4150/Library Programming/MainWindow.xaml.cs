using DigitalLibrary;
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
        ADAM4150 adam = null;

        public MainWindow()
        {
            InitializeComponent();

            adam = new ADAM4150(new ComSettingModel { DigitalQuantityCom = "COM2" });
        }

        private void btnReceive_Click(object sender, RoutedEventArgs e)
        {
            adam.SetData();

            lblDI0.Content = adam.DI0;
            lblDI1.Content = adam.DI1;
            lblDI2.Content = adam.DI2;
            lblDI3.Content = adam.DI3;
        }

        private void btnDO0_Click(object sender, RoutedEventArgs e)
        {
            if ((string)btnDO0.Content == "打开")
            {
                if (adam.OnOff(ADAM4150FuncID.OnDO0))
                    btnDO0.Content = "关闭";
            }
            else
            {
                if (adam.OnOff(ADAM4150FuncID.OffDO0))
                    btnDO0.Content = "打开";
            }
        }

        private void btnDO1_Click(object sender, RoutedEventArgs e)
        {
            if ((string)btnDO1.Content == "打开")
            {
                if (adam.OnOff(ADAM4150FuncID.OnDO1))
                    btnDO1.Content = "关闭";
            }
            else
            {
                if (adam.OnOff(ADAM4150FuncID.OffDO1))
                    btnDO1.Content = "打开";
            }
        }

        private void btnDO2_Click(object sender, RoutedEventArgs e)
        {
            if ((string)btnDO2.Content == "打开")
            {
                if (adam.OnOff(ADAM4150FuncID.OnDO2))
                    btnDO2.Content = "关闭";
            }
            else
            {
                if (adam.OnOff(ADAM4150FuncID.OffDO2))
                    btnDO2.Content = "打开";
            }
        }

    }
}
