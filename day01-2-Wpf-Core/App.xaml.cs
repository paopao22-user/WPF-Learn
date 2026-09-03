using System.Configuration;
using System.Data;
using System.Windows;

namespace day01_2_Wpf_Core
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);      // 先让框架干它的事（打开窗口）
            Console.WriteLine("程序启动");
            MessageBox.Show("欢迎使用本程序", "提示");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Console.WriteLine("程序退出");
            base.OnExit(e);      // 先让框架干它的事（关闭窗口）
            
            
        }
    }

}
