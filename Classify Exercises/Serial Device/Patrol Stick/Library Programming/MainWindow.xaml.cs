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

        List<string> cards = null;
        List<string> points = null;

        public MainWindow()
        {
            InitializeComponent();

            solid = new SOLID() { CommPort = 4 };

            cards = new List<string>();
            points = new List<string>();
        }

        private void btnGetAllData_Click(object sender, RoutedEventArgs e)
        {
            int count = solid.ReadDivceRecordWeb();

            Array array = Array.CreateInstance(typeof(string), count);

            if (solid.ReadDivceRecord(ref array) != count)
                return;

            foreach (string data in array)
            {
                lsbData.Items.Add(data);
            }
        }

        private void btnDeleteAllData_Click(object sender, RoutedEventArgs e)
        {
            string result = solid.DelDivceData();

            if (result != string.Empty)
                MessageBox.Show(result);
        }

        private void btnBindingCard_Click(object sender, RoutedEventArgs e)
        {
            string pointNo = solid.ReadDateWeb(0).Substring(14, 8);

            if (!points.Contains(pointNo))
                points.Add(pointNo);

            solid.DelDivceData();
        }

        private void btnBindingPoint_Click(object sender, RoutedEventArgs e)
        {
            string cardNo = solid.ReadDateWeb(0).Substring(14, 8);

            if (!cards.Contains(cardNo))
                cards.Add(cardNo);
        }

        private void btnAnalyze_Click(object sender, RoutedEventArgs e)
        {
            int count = solid.ReadDivceRecordWeb();

            Array array = Array.CreateInstance(typeof(string), count);

            if (solid.ReadDivceRecord(ref array) != count)
                return;

            lsbData.Items.Clear();

            string card = null;

            foreach (string data in array)
            {
                string cardNo = data.Substring(14, 8);
                string month, day, hour, minute, second;

                if (cards.Contains(cardNo))
                {
                    card = cardNo;

                    month = data.Substring(4, 2);
                    day = data.Substring(6, 2);
                    hour = data.Substring(8, 2);
                    minute = data.Substring(10, 2);
                    second = data.Substring(12, 2);

                    string record = string.Format("人员：{0} 于{1}月{2}日{3}时{4}分{5}秒打卡", card, month, day, hour, minute, second);
                    lsbData.Items.Add(record);
                }

                else if (card != null)
                    return;

                else if (points.Contains(cardNo))
                {
                    month = data.Substring(4, 2);
                    day = data.Substring(6, 2);
                    hour = data.Substring(8, 2);
                    minute = data.Substring(10, 2);
                    second = data.Substring(12, 2);

                    string record = string.Format("人员：{0} 于{1}月{2}日{3}时{4}分{5}秒在巡更点{6}打卡", card, month, day, hour, minute, second, cardNo);
                    lsbData.Items.Add(record);
                }
            }
        }
    }
}
