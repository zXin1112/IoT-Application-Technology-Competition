using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace Question2
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

        private void btnNewInfo_Click(object sender, RoutedEventArgs e)
        {
            NewInfo newInfo = new NewInfo();
            newInfo.ShowDialog();
        }

        private void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            Borrow borrow = new Borrow();
            borrow.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string connectionStr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                string sqlCommand = "select * from T_BookInfo";

                using(SqlDataAdapter adapter=new SqlDataAdapter(sqlCommand, connectionStr))
                {
                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    dgdBookInfo.ItemsSource = table.DefaultView;
                }
            }

        }
    }
}
