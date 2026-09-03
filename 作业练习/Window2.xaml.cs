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
using System.Windows.Shapes;

namespace 作业练习
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        private void rdoTcp_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if(radioButton != null)
            {
                string value = radioButton.Content.ToString();
                MessageBox.Show($"选择了{value}");
            }
        }

        private void rdoUdp_Checked(object sender, RoutedEventArgs e)
        {
            if(rdoTcp.IsChecked == true)
            {
                // 显示 TCP 参数
                // IP、端口可编辑

                // 串口、udp参数禁用
            }

            if (rdoUdp.IsChecked == true)
            {
                // 显示 UdP 参数
                // IP、端口可编辑

                // 串口、tcp参数禁用
            }

            if (rdock.IsChecked == true)
            {
                // 显示 串口  参数
                // IP、端口可编辑

                // tcp udp参数禁用
            }
        }

        private void rdock_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
