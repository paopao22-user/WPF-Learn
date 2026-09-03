using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace day01_1_Wpf_Framework
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Console.WriteLine("启动了程序");
            base.OnStartup(e);

        }

        protected override void OnExit(ExitEventArgs e)
        {
            Console.WriteLine("退出了程序");
            base.OnExit(e);
            // Perform any cleanup or resource release here if needed
        }

        protected override void OnActivated(EventArgs e)
        {
            Console.WriteLine("程序激活");
            base.OnActivated(e);
        }
    }
}
