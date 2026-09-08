using Prism.Ioc;
using PrismEventJournalDemo.Views;
using System.Windows;

namespace PrismEventJournalDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 注册三个导航用户控件
            containerRegistry.RegisterForNavigation<HomeView>();
            containerRegistry.RegisterForNavigation<DeviceView>();
            containerRegistry.RegisterForNavigation<AlarmView>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInitialized()
        {
            //启动默认首页
            base.OnInitialized();

            var regionManager = Container.Resolve<Prism.Regions.IRegionManager>();

            regionManager.RequestNavigate("MainRegion", "HomeView"); // 启动默认首页
        }
        
    }
}
