using day09_2Prism模板.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism模板.Views;
using System.Windows;

namespace day09_2Prism模板
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App:PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<LoginView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
