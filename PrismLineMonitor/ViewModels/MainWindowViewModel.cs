using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace PrismLineMonitor.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private readonly IRegionManager _regionManager;

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        public DelegateCommand<string> NavigateCommand { get; }


        private void Navigate(string viewName)
        {
            // 创建一个导航参数包
            NavigationParameters parameters = new NavigationParameters();

            // 如果去的是报警页面，就往参数包里面放数据
            if(viewName == "AlarmView")
            {
                parameters.Add("DeviceName", "PLC-01");
                parameters.Add("AlarmCount", 3);
            }

            // 导航时把参数包一起带过去
            _regionManager.RequestNavigate("MainRegion", viewName, parameters);


            
        }
    }
}
