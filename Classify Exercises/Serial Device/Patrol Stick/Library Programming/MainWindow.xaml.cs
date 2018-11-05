using Eluxun;
using System;
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

namespace Library_Programming
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SOLID solid = null;

        public MainWindow()
        {
            InitializeComponent();

            solid = new SOLID() { CommPort = 4 };
        }

        private void btnGetAllData_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
