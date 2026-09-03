using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace day01_2_Wpf_Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 打开第二个窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_OpenWindow(object sender, RoutedEventArgs e)
        {
            Window1 w = new Window1(); // 创建窗口对象
            w.Show();                   // 显示窗口,非模态窗口
        }


        private void Btn_Close(object sender, RoutedEventArgs e)
        {
            this.Close();       //只关闭当前窗口
        }
    }
}