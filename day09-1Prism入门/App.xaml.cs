using System.Windows;
using Prism.DryIoc;
using Prism.Ioc;
using day09_1Prism入门.Views;

namespace day09_1Prism入门
{
    /// <summary>
    /// 应用程序启动类
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// 创建应用的主窗口(shell)：告诉 Prism 主窗口是谁
        /// </summary>
        /// <returns></returns>
        /// 两个主要区别：RegisterTypes(...)对应注册；  Container.Resolve<MainWindow>()对应解析。
        /// RegisterTypes -> 先注册， CreateShell -> 需要 MainWindow, Container.Resolve -> 解析MainWindow，创建MainWindow
        protected override Window CreateShell()
        {
            //通过容器解析MainWindow（自动注入依赖项），返回主窗体。这里已经帮我们注册好容器了，自动
            //Resolve = “向容器要对象” Container = DI容器， Resolve = 解析 / 获取对象
            return Container.Resolve<MainWindow>();  // Container.Resolve(): 让容器负责创建主窗口

            ////手动创建版本：
            ////1.创建容器
            //var a = new DryIoc.Container();

            ////2.注册窗体
            //a.Register<MainWindow>();

            ////3.返回窗体
            //return a.Resolve<MainWindow>();
        }

        /// <summary>
        /// 先准备 DI 容器: 注册对象
        /// </summary>
        /// <param name="containerRegistry"></param>
        /// 可理解为：Prism，程序启动的时候让我先往你的 DI 容器里登记一些类型，RegisterTypes 就像“登记表”
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
