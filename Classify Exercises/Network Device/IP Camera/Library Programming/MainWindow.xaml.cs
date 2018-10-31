using IPCameraDll;
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

namespace Library_Programming
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread trdShowCamera;
        Timer trdSaveImage;
        Thread trdSaveVideo;

        IpCameraHelper ipCamera;

        bool logicalValue = true;
        bool savedLogical = false;

        bool isSaveImage = false;

        public MainWindow()
        {
            InitializeComponent();

            trdShowCamera = new Thread(new ThreadStart(MonitorLogic));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ipCamera = new IpCameraHelper("192.168.3.200:80", "admin", "", new Action<ImageEventArgs>((imageEventArgs) =>
            {
                imgCamera.Source = imageEventArgs.FrameReadyEventArgs.BitmapImage;

                if (isSaveImage)
                {

                }
            }));

            trdShowCamera.IsBackground = true;
            trdShowCamera.Start();
        }

        private void MonitorLogic()
        {
            while (true)
            {
                UpdateLogic();

                //if (ipCamera == null)
                //    Dispatcher.Invoke(new Action(() =>
                //    {

                //    }));

                if (savedLogical == logicalValue)
                    return;

                savedLogical = logicalValue;

                if (savedLogical)
                {
                    ipCamera.StartProcessing();
                }
                else
                {
                    ipCamera.StopProcessing();
                }
            }
        }

        private void UpdateLogic()
        {
            if ((DateTime.Now.Second % 5) == 0)
                logicalValue = !logicalValue;
        }

        private void btnAutoImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAutoVideo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUp_GotMouseCapture(object sender, MouseEventArgs e)
        {
            ipCamera.PanUp();
        }

        private void btnUp_LostMouseCapture(object sender, MouseEventArgs e)
        {
            ipCamera.PanUp();
        }

        private void btnDown_GotMouseCapture(object sender, MouseEventArgs e)
        {
            ipCamera.PanDown();
        }

        private void btnDown_LostMouseCapture(object sender, MouseEventArgs e)
        {
            ipCamera.PanDown();
        }

        private void btnLeft_GotMouseCapture(object sender, MouseEventArgs e)
        {
            ipCamera.PanLeft();
        }
        private void btnLeft_LostMouseCapture(object sender, MouseEventArgs e)
        {
            ipCamera.PanLeft();
        }

        private void btnRight_GotMouseCapture(object sender, MouseEventArgs e)
        {
            ipCamera.PanRight();
        }

        private void btnRight_LostMouseCapture(object sender, MouseEventArgs e)
        {
            ipCamera.PanRight();
        }
    }
}
