using DigitalLibrary;
using LEDLibrary;
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
using ZigBeeLibrary;

namespace Question2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer timer;

        ZigBee zigBee;
        ADAM4150 adam;

        bool OnFire;

        public MainWindow()
        {
            InitializeComponent();

            timer = new Timer();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            zigBee = new ZigBee(new ZigBeeLibrary.ComSettingModel() { ZigbeeCom = "COM4" });
            adam = new ADAM4150(new DigitalLibrary.ComSettingModel() { DigitalQuantityCom = "COM5" });

            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Interval = 1000;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MonitorZigBee();
            MonitorADAM();
        }

        private void MonitorZigBee()
        {
            zigBee.GetSet();

            this.Dispatcher.Invoke(new Action(() =>
            {
                lblTemp.Content = zigBee.lightValue;
                lblHum.Content = zigBee.temperatureValue;
                lblLig.Content = zigBee.lightValue;
            }));

            double lightNum, limitNum;

            if (double.TryParse(zigBee.lightValue, out lightNum) && double.TryParse(txbLig.Text, out limitNum))
            {
                if (lightNum < limitNum)
                {
                    adam.OnOff(ADAM4150FuncID.OffDO2);
                }
                else
                {
                    adam.OnOff(ADAM4150FuncID.OnDO2);
                }
            }
        }

        private void MonitorADAM()
        {
            adam.SetData();

            this.Dispatcher.Invoke(new Action(() =>
            {
                lblFla.Content = adam.DI1 ? "检测到火焰" : "未检测到火焰";
                lblSmo.Content = adam.DI2 ? "检测到烟雾" : "未检测到烟雾";
            }));

            if (adam.DI1 == OnFire)
                return;

            OnFire = adam.DI1;

            LEDPlayer ledPlayer = new LEDPlayer("COM2");

            if (adam.DI1)
            {
                adam.OnOff(ADAM4150FuncID.OnDO1);

                ledPlayer.DisplayText("厨房着火");
            }
            else
            {
                adam.OnOff(ADAM4150FuncID.OffDO1);

                ledPlayer.DisplayText("一切正常");
            }
        }
    }
}
