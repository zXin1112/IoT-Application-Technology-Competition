using AForge.Video.FFMPEG;
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
        string ledCom, ledText, cameraIp;
        int recordLength;

        Thread thread;
        System.Timers.Timer timer = new System.Timers.Timer();

        IpCameraHelper ipCameraHelper;
        VideoFileWriter videoFileWriter;

        public MainWindow()
        {
            InitializeComponent();

            videoFileWriter = new VideoFileWriter();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("D:\videos"))
                Directory.CreateDirectory("D:\videos");

            if (!File.Exists("Config.xml"))
                return;

            XmlDocument document = new XmlDocument();
            document.Load("Config.xml");

            XmlNode eleLED = document.SelectSingleNode("/root/LED");
            ledCom = eleLED.Attributes["Com"].Value;
            ledText = eleLED.Attributes["Text"].Value;

            XmlNode eleCamera = document.SelectSingleNode("/root/Camera");
            cameraIp = eleCamera.Attributes["IP"].Value;
            recordLength = int.Parse(eleCamera.Attributes["RecordLength"].Value);

            thread = new Thread(new ThreadStart(MonitorSensor));

            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Interval = recordLength * 100;
            timer.Start();
        }

        public void MonitorSensor()
        {
            //adam.SetData();

            if ((int)btnStart.Tag == 0)
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
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string filePath = @"D:\videos\" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".mp4";
            File.Create(filePath);

            if (!videoFileWriter.IsOpen)
            {
                videoFileWriter.Open(filePath, 640, 840);
            }
            else
            {
                videoFileWriter.Close();
            }

        }

        public void ShowImage(ImageEventArgs imageEventArgs)
        {
            imgCamera.Source = imageEventArgs.FrameReadyEventArgs.BitmapImage;

            //BitmapImage image = imageEventArgs.FrameReadyEventArgs.BitmapImage;

            //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(image.PixelWidth, image.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            //System.Drawing.Imaging.BitmapData data = bmp.LockBits(
            //new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            //image.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride); bmp.UnlockBits(data);

            MemoryStream stream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageEventArgs.FrameReadyEventArgs.BitmapImage));
            encoder.Save(stream);

            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(stream);
            stream.Close();


            if (videoFileWriter.IsOpen)
                videoFileWriter.WriteVideoFrame(bitmap);
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

        private void btnAutoLED_Click(object sender, RoutedEventArgs e)
        {
            LEDPlayer player = new LEDPlayer(ledCom);
            player.DisplayText(ledText);
        }
    }
}
