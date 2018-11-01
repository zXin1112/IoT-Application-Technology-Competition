using IPCameraDll;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        Timer trdSaveVideo;

        IpCameraHelper ipCamera;

        bool logicalValue = true;
        bool savedLogical = false;

        bool isSaveImage = false;

        public MainWindow()
        {
            InitializeComponent();

            ipCamera = new IpCameraHelper("192.168.3.200:80", "admin", "", new Action<ImageEventArgs>((imageEventArgs) =>
            {
                imgCamera.Source = imageEventArgs.FrameReadyEventArgs.BitmapImage;
            }));

            trdShowCamera = new Thread(new ThreadStart(MonitorLogic));
            trdShowCamera.IsBackground = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            trdShowCamera.Start();
        }

        private void MonitorLogic()
        {
            while (true)        //线程循环监测
            {
                UpdateLogic();      //更新逻辑值状态

                if (savedLogical == logicalValue)       //逻辑值状态未改变时跳出
                    return;

                savedLogical = logicalValue;        //逻辑值已改变，保存逻辑值到变量

                if (savedLogical)
                {
                    ipCamera.StartProcessing();
                }
                else
                {
                    ipCamera.StopProcessing();
                }

                Thread.Sleep(1000);
            }
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

        private void SaveImageInFile()
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();        //实例化.jpg格式的编码器

            BitmapFrame bitmapFrame = BitmapFrame.Create((BitmapImage)imgCamera.Source);        //格式化图像数据
            encoder.Frames.Add(bitmapFrame);

            using (FileStream stream = new FileStream(DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".jpg", FileMode.CreateNew))       //创建要保存的文件并打开流
                encoder.Save(stream);
        }

        private void ReadImageFormFile(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                BitmapImage image = new BitmapImage();

                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();

                imgCamera.Source = image;
            }
        }

        private void SaveImageInDatabase()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=192.168.1.2;Initial Catalog=TestCamera;Persist Security Info=True;User ID=sa;Password=123456"))
            {
                using (SqlCommand command = new SqlCommand("insert into T_Camera values (@image)", connection))
                {
                    using (Stream stream = ((BitmapImage)imgCamera.Source).StreamSource)
                    {
                        stream.Position = 0;

                        connection.Open();
                        command.Parameters.AddWithValue("@image", stream);

                        int result = command.ExecuteNonQuery();

                        if (result == 1)
                            MessageBox.Show("成功");
                    }
                }
            }
        }

        private void ReadImageFormDatabase()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=192.168.1.2;Initial Catalog=TestCamera;Persist Security Info=True;User ID=sa;Password=123456"))
            {
                using (SqlCommand command = new SqlCommand("select CameraImage from T_Camera where No=5", connection))
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    BitmapImage image = new BitmapImage();

                    image.BeginInit();
                    image.StreamSource = new MemoryStream((byte[])reader.GetValue(0));
                    image.EndInit();

                    imgCamera.Source = image;
                }
            }
        }

        int i = 0;

        private void UpdateLogic()
        {
            if (i > 10)
            {
                logicalValue = !logicalValue;
                i = 0;
            }
            else
            {
                i++;
            }
        }
    }
}
