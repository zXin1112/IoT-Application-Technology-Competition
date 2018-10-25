using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
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
using System.Windows.Shapes;
using System.Xml;

namespace Question1
{
    /// <summary>
    /// widSetUp.xaml 的交互逻辑
    /// </summary>
    public partial class widSetUp : Window
    {
        public widSetUp()
        {
            InitializeComponent();

            cmbLEDCom.ItemsSource = SerialPort.GetPortNames();
            cmbLEDCom.SelectedIndex = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("Config.xml"))
                return;

            XmlDocument document = new XmlDocument();
            document.Load("Config.xml");

            XmlNode eleLED = document.SelectSingleNode("/root/LED");
            cmbLEDCom.SelectedItem = eleLED.Attributes["Com"].Value;
            txbLed.Text = eleLED.Attributes["Text"].Value;

            XmlNode eleCamera = document.SelectSingleNode("/root/Camera");
            txbIP.Text = eleCamera.Attributes["IP"].Value;
            txbTime.Text = eleCamera.Attributes["RecordLength"].Value;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            int time;

            if (cmbLEDCom.Items.Count == 0)
            {
                MessageBox.Show("无串口选择！");
                return;
            }

            if(!int.TryParse(txbTime.Text,out time))
            {
                MessageBox.Show("录制时长错误！");
                return;
            }

            XmlDocument document = new XmlDocument();

            XmlElement root = document.CreateElement("root");
            document.AppendChild(root);

            XmlElement eleLED = document.CreateElement("LED");
            eleLED.SetAttribute("Com", cmbLEDCom.SelectedItem.ToString());
            eleLED.SetAttribute("Text", txbLed.Text);

            XmlElement eleCamera = document.CreateElement("Camera");
            eleCamera.SetAttribute("IP", txbIP.Text);
            eleCamera.SetAttribute("RecordLength", txbTime.Text);

            root.AppendChild(eleLED);
            root.AppendChild(eleCamera);

            document.Save("Config.xml");

            this.Close();
        }
    }
}
