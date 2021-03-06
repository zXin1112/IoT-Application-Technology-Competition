﻿using DigitalLibrary;
using IPCameraDll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Question1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread thread;

        ADAM4150 adam;
        IpCameraHelper ipCameraHelper;

        bool adamDIO = true;

        public MainWindow()
        {
            InitializeComponent();

            thread = new Thread(new ThreadStart(MonitorSensor));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            adam = new ADAM4150(new ComSettingModel() { DigitalQuantityCom = "COM5" });

            thread.Start();

            if (!Directory.Exists(@"Image\"))
            {
                Directory.CreateDirectory(@"Image\");
            }
        }

        public void MonitorSensor()
        {
            if (ipCameraHelper == null)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    ipCameraHelper = new IpCameraHelper("192.168.3.200:80", "admin", "", new Action<ImageEventArgs>(ShowImage));
                }));
            }

            //adam.SetData();

            if (adamDIO == adam.DI0)
                return;

            adamDIO = adam.DI0;

            if (!adamDIO)
            {
                ipCameraHelper.StartProcessing();
            }
            else
            {
                if (ipCameraHelper != null)
                    ipCameraHelper.StopProcessing();
            }

            MonitorSensor();
        }

        public void ShowImage(ImageEventArgs imageEventArgs)
        {
            imgCamera.Source = imageEventArgs.FrameReadyEventArgs.BitmapImage;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var bitmapEncoder = new JpegBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapImage)imgCamera.Source));

            using (FileStream fileStream = new FileStream(@"Image\人员截图" + DateTime.Now.ToString("hhmmss") + ".jpg", FileMode.CreateNew))
            {
                bitmapEncoder.Save(fileStream);
            }
        }

        private void btnU_MouseDown(object sender, MouseEventArgs e)
        {
            ipCameraHelper.PanUp();
        }

        private void btnU_MouseUp(object sender, MouseEventArgs e)
        {
            ipCameraHelper.PanUp();
        }

        private void btnD_MouseDown(object sender, MouseEventArgs e)
        {
            ipCameraHelper.PanDown();
        }

        private void btnD_MouseUp(object sender, MouseEventArgs e)
        {
            ipCameraHelper.PanDown();
        }

        private void btnL_MouseDown(object sender, MouseEventArgs e)
        {
            ipCameraHelper.PanLeft();
        }

        private void btnL_MouseUp(object sender, MouseEventArgs e)
        {
            ipCameraHelper.PanLeft();
        }

        private void btnR_MouseDown(object sender, MouseEventArgs e)
        {
            ipCameraHelper.PanRight();
        }

        private void btnR_MouseUp(object sender, MouseEventArgs e)
        {
            ipCameraHelper.PanRight();
        }
    }
}
