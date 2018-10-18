using Srr1100U;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace Question2
{
    /// <summary>
    /// NewInfo.xaml 的交互逻辑
    /// </summary>
    public partial class NewInfo : Window
    {
        SrrReader srrReader;

        public NewInfo()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbPort.ItemsSource = SerialPort.GetPortNames();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if ((int)btnOpen.Tag == 0)
            {
                srrReader = new SrrReader(cmbPort.SelectedItem.ToString());
                srrReader.ConnDevice();
                srrReader.Read(new Action<string>(ReadRfid));

                btnOpen.Tag = 1;
                btnOpen.Content = "关闭串口";
            }
            else
            {
                srrReader.CloseDevice();
                btnOpen.Tag = 0;
                btnOpen.Content = "打开串口";
            }
        }

        private void ReadRfid(string data)
        {
            txbCardNo.Text = data.Trim();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string connectionStr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                string sqlCommand = "insert into T_BookInfo values (@name,@no,@status)";

                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    command.Parameters.Add(new SqlParameter("@name", txbName.Text.Trim()));
                    command.Parameters.Add(new SqlParameter("@no", txbCardNo.Text.Trim()));
                    command.Parameters.Add(new SqlParameter("@status", 1));

                    int result = command.ExecuteNonQuery();

                    if (result == 1)
                        MessageBox.Show("保存成功");
                    else
                        MessageBox.Show("保存失败");
                }
            }
        }
    }
}
