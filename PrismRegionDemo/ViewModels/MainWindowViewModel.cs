using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismRegionDemo.ViewModels
{
    public class MainWindowViewModel:BindableBase
    {
        //依赖注入：IRegionManager 的自动流入
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; }


        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string viewName)
        {
            //RequestNavigate（请求导航方法）：向管家发出一道调动令。
            //第一个参数 "ContentRegion"：指定目的地 —— 在哪个画框里呈现？（对应 MainWindow.xaml 中的 prism: RegionManager.RegionName = "ContentRegion"）。
            //第二个参数 viewName：指定调动对象 —— 把哪一幅画挂上去？（对应已经在 App.xaml.cs 中登记过的页面名字）。
            _regionManager.RequestNavigate("ContentRegion", viewName);
        }
    }
}
