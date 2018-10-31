using DigitalLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test_A
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer timer = null;
        ADAM4150 adam = null;

        bool isHum=true;

        public MainWindow()
        {
            InitializeComponent();

            adam = new ADAM4150(new ComSettingModel { DigitalQuantityCom = "COM3" });

            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            adam.SetData();

            if (isHum == adam.DI0)
                return;

            isHum = adam.DI0;

            if (!isHum)
            {
                adam.OnOff(ADAM4150FuncID.OnDO0);
            }
            else
            {
                adam.OnOff(ADAM4150FuncID.OffDO0);
            }
        }
    }
}
