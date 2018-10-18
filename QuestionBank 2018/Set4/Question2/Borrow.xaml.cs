using MWRDemoDll;
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
    /// Borrow.xaml 的交互逻辑
    /// </summary>
    public partial class Borrow : Window
    {
        SrrReader srrReader;

        public Borrow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbPort.ItemsSource = SerialPort.GetPortNames();
        }

        private void btnOpenPort_Click(object sender, RoutedEventArgs e)
        {
            if ((int)btnOpenPort.Tag == 0)
            {
                srrReader = new SrrReader(cmbPort.SelectedItem.ToString());
                srrReader.ConnDevice();
                srrReader.Read(new Action<string>(ReadRfid));

                btnOpenPort.Tag = 1;
                btnOpenPort.Content = "关闭串口";
            }
            else
            {
                srrReader.CloseDevice();
                btnOpenPort.Tag = 0;
                btnOpenPort.Content = "打开串口";
            }
        }

        private void ReadRfid(string data)
        {
            txbBookNo.Text = data.Trim();

            string connectionStr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                string sqlCommand = "select * from T_BookInfo where BookNo=@no";

                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    command.Parameters.Add(new SqlParameter("@no", txbBookNo.Text.Trim()));

                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    txbBookName.Text = reader.GetString(0).Trim();
                    txbStatus.Text = reader.GetString(2).Trim() == "1" ? "正常" : "已借出";
                }
            }
        }

        private void btnReadCard_Click(object sender, RoutedEventArgs e)
        {
            MifareRFEYE.Instance.ConnDevice();
            MifareRFEYE.Instance.Search();

            txbStuNo.Text = MifareRFEYE.Instance.ReadString();

            MifareRFEYE.Instance.CloseDevice();

            string connectionStr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                string sqlCommand = "select * from T_UserInfo where CardNo=@no";

                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    command.Parameters.Add(new SqlParameter("@no", txbCardNo.Text.Trim()));

                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    txbStuName.Text = reader.GetString(1).Trim();
                    txbStuNo.Text = reader.GetString(2).Trim();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string connectionStr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                string sqlCommand = "insert into T_BorrowBook values (@bookNo,@stuNo,@add)";

                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    command.Parameters.Add(new SqlParameter("@bookNo", txbBookNo.Text.Trim()));
                    command.Parameters.Add(new SqlParameter("@stuNo", txbCardNo.Text.Trim()));
                    command.Parameters.Add(new SqlParameter("@add", DateTime.Now));

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
