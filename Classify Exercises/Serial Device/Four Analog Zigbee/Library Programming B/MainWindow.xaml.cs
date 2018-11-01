﻿using System;
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
using ZigBeeLibrary;

namespace Library_Programming_B
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ZigBee fourAnalogZigbee = null;

        public MainWindow()
        {
            InitializeComponent();

            fourAnalogZigbee = new ZigBee(new ComSettingModel { ZigbeeCom = "COM4" });
        }

        private void btnGetData_Click(object sender, RoutedEventArgs e)
        {
            fourAnalogZigbee.GetSet();

            byte[] data = fourAnalogZigbee.OriginalData;

            int value = BitConverter.ToInt32(data, 0);
        }
    }
}