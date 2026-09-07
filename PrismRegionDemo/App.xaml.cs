using Prism.DryIoc;
using Prism.Ioc;
using PrismRegionDemo.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PrismRegionDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<HomeView>();
            containerRegistry.RegisterForNavigation<DeviceView>();
            containerRegistry.RegisterForNavigation<SettingView>();
        }
    }

}
