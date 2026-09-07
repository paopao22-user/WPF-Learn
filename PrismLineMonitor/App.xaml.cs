using Prism.Ioc;
using Prism.Regions;
using PrismLineMonitor.Views;
using System.Windows;

namespace PrismLineMonitor
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
            //注册三个UserControl到导航中，方便后续使用RegionManager进行导航
            containerRegistry.RegisterForNavigation<OverviewView>();
            containerRegistry.RegisterForNavigation<AlarmView>();
            containerRegistry.RegisterForNavigation<SettingView>();
            containerRegistry.RegisterForNavigation<DeviceView>();
            containerRegistry.RegisterForNavigation<DeviceDetailView>();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            // 第 1 步：底层点火
            base.OnInitialized(); // base.OnInitialized():调用父类 PrismApplication 内部的初始化逻辑

            // 第 2 步：向容器索要大管家
            var regionManager = Container.Resolve<IRegionManager>();

            // 第 3 步：把默认首页送进画框
            regionManager.RequestNavigate("MainRegion", "OverviewView");
        }
    }
}
