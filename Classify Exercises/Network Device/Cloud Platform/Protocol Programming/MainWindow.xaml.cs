using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace Protocol_Programming
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("http://gateway.nlecloud.com/api/Account/Logon");
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.Accept = "application/text";
            request.Method = "POST";
            request.Headers.Add("AccessToken", "");
            request.ContentType = "application/json";

            ClientLogonData clientLogonData = new ClientLogonData();
            clientLogonData.UserName = "ytvciot";
            clientLogonData.Password = "123456";
            clientLogonData.ProjectTag = "CloudTest";

            string jsonData = JsonConvert.SerializeObject(clientLogonData);
            txbTest.Text = jsonData;
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string showdata = streamReader.ReadToEnd();
            txbTest.Text = showdata;
            streamReader.Close();

            OperationResult result = JsonConvert.DeserializeObject<OperationResult>(showdata);
            ClientLogonResult clientLogonResult = JsonConvert.DeserializeObject<ClientLogonResult>(result.AppendData.ToString());
            txbTest.Text = clientLogonResult.AccessToken;
            token = clientLogonResult.AccessToken;
        }

        private void btnSensorData_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("http://gateway.nlecloud.com/api/SensorData/GetNewestSensorData?sensorId=65300");
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.Accept = "application/text";
            request.Method = "POST";
            request.Headers.Add("AccessToken", token);
            request.ContentType = "application/json";

            string jsonData = JsonConvert.SerializeObject("AAA");
            //txbTest.Text = jsonData;
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string showdata = streamReader.ReadToEnd();
            txbTest.Text = showdata;
            streamReader.Close();

            OperationResult result = JsonConvert.DeserializeObject<OperationResult>(showdata);
            SensorDataModel sensorDataModel = JsonConvert.DeserializeObject<SensorDataModel>(result.AppendData.ToString());
            txbTest.Text = sensorDataModel.SensorData + sensorDataModel.SensorUnit;

            Uri uri = new Uri("http://gateway.nlecloud.com/api/Device/GetSensorList");
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.Accept = "application/text";
            request.Method = "POST";
            request.Headers.Add("AccessToken", token);
            //request.Headers.Add("AccessToken", "");
            request.ContentType = "application/json";

            string jsonData = JsonConvert.SerializeObject("AAA");
            //txbTest.Text = jsonData;
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            Stream stream = request.GetRequestStream();
            //stream.Write(data, 0, data.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string showdata = streamReader.ReadToEnd();
            txbTest.Text = showdata;
            streamReader.Close();

            OperationResult result = JsonConvert.DeserializeObject<OperationResult>(showdata);
            List<SensorModelBase> lsSensorModelBase = JsonConvert.DeserializeObject<List<SensorModelBase>>(result.AppendData.ToString());
            //txbTest.Text = sensorDataModel.SensorData + sensorDataModel.SensorUnit;
        }
    }
}
