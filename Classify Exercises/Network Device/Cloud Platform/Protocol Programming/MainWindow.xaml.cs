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
        string token = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private string RequestApi(string apiAddress, object requestData)
        {
            Uri uri = new Uri("http://172.18.10.11:81/" + apiAddress);
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.Method = "POST";
            request.Headers.Add("AccessToken", token);
            request.ContentType = "application/json";

            string jsonData = JsonConvert.SerializeObject(requestData);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            using (Stream stream = request.GetRequestStream())
                stream.Write(data, 0, data.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                return streamReader.ReadToEnd();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string jsonData = RequestApi("api/Account/Logon", new ClientLogonData() { UserName = "user10", Password = "123456", ProjectTag = "Test" });

            OperationResult<ClientLogonResult> result = JsonConvert.DeserializeObject<OperationResult<ClientLogonResult>>(jsonData);

            if (result.Status != ResultStatus.Success)
                return;

            ClientLogonResult clientLogonResult = result.AppendData;
            token = clientLogonResult.AccessToken;
        }



        private void btnSensorData_Click(object sender, RoutedEventArgs e)
        {
            string jsonData = RequestApi("api/SensorData/GetNewestSensorDatas", new int[] { 135, 137, 139, 140, 138, 136 });

            OperationResult<List<SensorDataModel>> result = JsonConvert.DeserializeObject<OperationResult<List<SensorDataModel>>>(jsonData);
            List<SensorDataModel> sensorDataModel = result.AppendData;

            foreach (SensorDataModel model in sensorDataModel)
            {
                switch (model.SensorId)
                {
                    case 135: lbl02IN1.Content = model.SensorData + " " + model.SensorUnit; break;
                    case 137: lbl02IN2.Content = model.SensorData + " " + model.SensorUnit; break;
                    case 139: lbl01IN2.Content = model.SensorData + " " + model.SensorUnit; break;
                    case 140: lbl01IN3.Content = model.SensorData + " " + model.SensorUnit; break;
                    case 138: lbl02IN3.Content = model.SensorData + " " + model.SensorUnit; break;
                    case 136: lbl01IN4.Content = model.SensorData + " " + model.SensorUnit; break;
                }
            }
        }

        private void btnLamp01_Click(object sender, RoutedEventArgs e)
        {
            if (btnLamp01.Tag.ToString() == "0")
            {
                string jsonData = RequestApi("api/Device/OperationActuator?actuatorId=1&actuatorStatus=1", " ");

                OperationResult<string> result = JsonConvert.DeserializeObject<OperationResult<string>>(jsonData);

                if (result.Status == ResultStatus.Success)
                {
                    btnLamp01.Tag = "1";
                    btnLamp01.Content = "关";
                }

                txbJson.Text = result.Message;
            }
            else
            {
                string jsonData = RequestApi("api/Device/OperationActuator?actuatorId=1&actuatorStatus=0", " ");

                OperationResult<string> result = JsonConvert.DeserializeObject<OperationResult<string>>(jsonData);

                if (result.Status == ResultStatus.Success)
                {
                    btnLamp01.Tag = "0";
                    btnLamp01.Content = "开";
                }

                txbJson.Text = result.Message;

            }
        }

        private void btnLamp02_Click(object sender, RoutedEventArgs e)
        {
            if (btnLamp02.Tag.ToString() == "0")
            {
                string jsonData = RequestApi("api/Device/OperationActuator?actuatorId=2&actuatorStatus=1", " ");

                OperationResult<string> result = JsonConvert.DeserializeObject<OperationResult<string>>(jsonData);

                if (result.Status == ResultStatus.Success)
                {
                    btnLamp02.Tag = "1";
                    btnLamp02.Content = "关";
                }

                txbJson.Text = result.Message;
            }
            else
            {
                string jsonData = RequestApi("api/Device/OperationActuator?actuatorId=2&actuatorStatus=0", " ");

                OperationResult<string> result = JsonConvert.DeserializeObject<OperationResult<string>>(jsonData);

                if (result.Status == ResultStatus.Success)
                {
                    btnLamp02.Tag = "0";
                    btnLamp02.Content = "开";
                }

                txbJson.Text = result.Message;
            }
        }
    }
}
