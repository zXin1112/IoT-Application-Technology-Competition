using IPCameraDll;
using LEDLibrary;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;

namespace Question1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string ledCom, ledText, cameraIp, recordLength;

        Thread thread;

        IpCameraHelper ipCameraHelper;

        public MainWindow()
        {
            InitializeComponent();

            thread = new Thread(new ThreadStart(MonitorSensor));
        }

        public void MonitorSensor()
        {
            //adam.SetData();

            if ((int)btnStart.Tag==0)
            {
                if (ipCameraHelper == null)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        ipCameraHelper = new IpCameraHelper(cameraIp, "admin", "", new Action<ImageEventArgs>(ShowImage));
                    }));
                }

                ipCameraHelper.StartProcessing();

                btnStart.Content = "停止监控";
                btnStart.Tag = 1;
            }
            else
            {
                if (ipCameraHelper != null)
                    ipCameraHelper.StopProcessing();

                btnStart.Content = "开始监控";
                btnStart.Tag = 0;
            }

            MonitorSensor();
        }

        public void ShowImage(ImageEventArgs imageEventArgs)
        {
            imgCamera.Source = imageEventArgs.FrameReadyEventArgs.BitmapImage;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("Config.xml"))
                return;

            XmlDocument document = new XmlDocument();
            document.Load("Config.xml");

            XmlNode eleLED = document.SelectSingleNode("/root/LED");
            ledCom = eleLED.Attributes["Com"].Value;
            ledText = eleLED.Attributes["Text"].Value;

            XmlNode eleCamera = document.SelectSingleNode("/root/Camera");
            cameraIp = eleCamera.Attributes["IP"].Value;
            recordLength = eleCamera.Attributes["RecordLength"].Value;
        }

        private void btnAutoLED_Click(object sender, RoutedEventArgs e)
        {
            LEDPlayer player = new LEDPlayer(ledCom);
            player.DisplayText(ledText);
        }

        private void btnSetUp_Click(object sender, RoutedEventArgs e)
        {
            widSetUp setUp = new widSetUp();
            setUp.ShowDialog();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            thread.Start();
        }
    }
}
