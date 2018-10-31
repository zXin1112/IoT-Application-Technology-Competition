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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create((BitmapImage)imgCamera.Source));

            //FileStream stream = new FileStream(DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".jpg", FileMode.CreateNew);
            //encoder.Save(stream);

            //stream.Close();

            ShowImage();
        }

        private void SaveImage()
        {
            using (SqlConnection con = new SqlConnection("Data Source=192.168.1.2;Initial Catalog=TestCamera;Persist Security Info=True;User ID=sa;Password=123456"))
            {
                using (SqlCommand com = new SqlCommand("insert into T_Camera values (@image)", con))
                {
                    using (Stream stream = ((BitmapImage)imgCamera.Source).StreamSource)
                    {
                        stream.Position = 0;

                        con.Open();
                        com.Parameters.AddWithValue("@image", stream);

                        int result = com.ExecuteNonQuery();
                        if (result == 1)
                            MessageBox.Show("成功");
                    }
                }
            }
        }

        private void ShowImage()
        {
            using (SqlConnection con = new SqlConnection("Data Source=192.168.1.2;Initial Catalog=TestCamera;Persist Security Info=True;User ID=sa;Password=123456"))
            {
                using (SqlCommand com = new SqlCommand("select CameraImage from T_Camera where No=5", con))
                {


                    con.Open();

                    SqlDataReader reader = com.ExecuteReader();
                    reader.Read();
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = new MemoryStream((byte[])reader.GetValue(0));
                    image.EndInit();

                    imgCamera.Source = image;




                }
            }
        }
    }
}
