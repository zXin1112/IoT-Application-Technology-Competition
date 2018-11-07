using Newtonsoft.Json;
using Protocol_Programming.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
        string token;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("http://172.18.10.11:81/api/Account/Logon");
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.Accept = "application/text";
            request.Method = "POST";
            request.Headers.Add("AccessToken", "");
            request.ContentType = "application/json";

            ClientLogonData clientLogonData = new ClientLogonData();
            clientLogonData.UserName = "user10";
            clientLogonData.Password = "123456";
            clientLogonData.ProjectTag = "Test";

            string jsonData = JsonConvert.SerializeObject(clientLogonData);
            txbJson.Text = jsonData;
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            using (Stream stream = request.GetRequestStream())
                stream.Write(data, 0, data.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string showdata = streamReader.ReadToEnd();
            txbJson.Text = showdata;
            streamReader.Close();

            OperationResult<ClientLogonResult> result = JsonConvert.DeserializeObject<OperationResult<ClientLogonResult>>(showdata);
            ClientLogonResult clientLogonResult = result.AppendData;
            txbJson.Text = clientLogonResult.AccessToken;
            token = clientLogonResult.AccessToken;
        }

        private void btnSensorData_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("http://172.18.10.11:81/api/SensorData/GetNewestSensorData?sensorId=135");
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.Accept = "application/text";
            request.Method = "POST";
            request.Headers.Add("AccessToken", token);
            request.ContentType = "application/json";

            string jsonData = JsonConvert.SerializeObject("123");
            //txbTest.Text = jsonData;
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            using (Stream stream = request.GetRequestStream())
                stream.Write(data, 0, data.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string showdata = streamReader.ReadToEnd();
            txbJson.Text = showdata;
            streamReader.Close();

            OperationResult<SensorDataModel> result = JsonConvert.DeserializeObject<OperationResult<SensorDataModel>>(showdata);
            SensorDataModel sensorDataModel = result.AppendData;
            txbJson.Text = sensorDataModel.SensorData + sensorDataModel.SensorUnit;

            //Uri uri2 = new Uri("http://172.18.10.11:81/api/Device/GetSensorList");
            //HttpWebRequest request2 = HttpWebRequest.CreateHttp(uri2);
            //request2.Accept = "application/text";
            //request2.Method = "POST";
            //request2.Headers.Add("AccessToken", token);
            ////request.Headers.Add("AccessToken", "");
            //request2.ContentType = "application/json";

            //string jsonData2 = JsonConvert.SerializeObject("AAA");
            ////txbTest.Text = jsonData;
            //byte[] data2 = Encoding.UTF8.GetBytes(jsonData2);

            //Stream stream2 = request2.GetRequestStream();
            ////stream.Write(data, 0, data.Length);
            //stream.Close();

            //HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
            //StreamReader streamReader = new StreamReader(response2.GetResponseStream(), Encoding.UTF8);
            //string showdata = streamReader.ReadToEnd();
            //txbTest.Text = showdata;
            //streamReader.Close();

            //OperationResult result = JsonConvert.DeserializeObject<OperationResult>(showdata);
            //List<SensorModelBase> lsSensorModelBase = JsonConvert.DeserializeObject<List<SensorModelBase>>(result.AppendData.ToString());
            ////txbTest.Text = sensorDataModel.SensorData + sensorDataModel.SensorUnit;
        }

        private void btnFan01_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("http://172.18.10.11:81/api/Device/OperationActuator?actuatorId=1&actuatorStatus=0");
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.Accept = "application/text";
            request.Method = "POST";
            request.Headers.Add("AccessToken", token);
            request.ContentType = "application/json";

            string jsonData = JsonConvert.SerializeObject("123");
            //txbTest.Text = jsonData;
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string showdata = streamReader.ReadToEnd();
            txbJson.Text = showdata;
            streamReader.Close();

            OperationResult<string> result = JsonConvert.DeserializeObject<OperationResult<string>>(showdata);
            string sensorDataModel = result.AppendData;
            txbJson.Text = sensorDataModel;
        }
    }
}
