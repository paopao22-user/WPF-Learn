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
using System.IO;
using System.Threading;

namespace day01_4_Shutdown_Exit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool _threadRunning = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 优雅退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Shutdown(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); //
        }

        /// <summary>
        /// 强制退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Exit(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);     //强制退出
        }

        // 后台线程：每 0.5 秒往桌面日志写一行，共 10 行后自己结束
        private void Btn_StartThread(object sender, RoutedEventArgs e)
        {
            // 如果线程在运行中直接返回
            if (_threadRunning)
            {
                return;
            }
            //修改运行状态
            _threadRunning = true;

            //合并路径
            string logPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "退出程序练习日志.txt");

            File.WriteAllText(logPath, "=== 后台线程开始 ===\r\n");

            Thread t = new Thread(() => 
            {
                for (int i = 1; i <= 10; i++)
                {
                    File.AppendAllText(logPath, $"第 {i} 行：后台线程还在工作...\r\n");
                    Thread.Sleep(500);
                }
                File.AppendAllText(logPath, "=== 后台线程结束 ===\r\n");
            });
            
            t.IsBackground = false;   // 前台线程：程序退出前必须等它干完活
            t.Start();
        }
    }
}