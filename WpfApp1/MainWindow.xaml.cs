using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfApp1
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"1.点击事件线程:{Thread.CurrentThread.ManagedThreadId}");

            //1.启动后台任务  把任务交给线程池去执行，因此会在后台线程中执行
            await Task.Run(async() =>
            {
                Console.WriteLine($"2.Task.Run开始:{Thread.CurrentThread.ManagedThreadId}");
                // 后台任务中等待 2 秒
                await Task.Delay(2000);

                Console.WriteLine($"3.Delay以后:{Thread.CurrentThread.ManagedThreadId}");

                // 此时仍然需要回 UI 线程修改控件
                btn.Dispatcher.Invoke(() =>
                {
                    Console.WriteLine($"4.DisPatcher线程里面:{Thread.CurrentThread.ManagedThreadId}");
                    btn.Content = "完成";
                });
                Console.WriteLine($"5.DisPathcer线程以后:{Thread.CurrentThread.ManagedThreadId}");
            });
            Console.WriteLine($"6.Task.Run结束以后:{Thread.CurrentThread.ManagedThreadId}");
        }

    }
}
