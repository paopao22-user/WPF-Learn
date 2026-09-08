using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismLineMonitor.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismLineMonitor.ViewModels
{
    public class DeviceViewModel:BindableBase
    {
        private readonly IRegionManager _regionManager;         //负责导航

        private readonly IEventAggregator _eventAggregator;     //负责发消息

        //  构造函数注入事件聚合器大管家
        public DeviceViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;

            _eventAggregator = eventAggregator;

            DetailCommand = new DelegateCommand<string>(OpenDetail);

            AlarmCommand = new DelegateCommand<string>(SendAlarm);
        }

        //声明命令
        public DelegateCommand<string> DetailCommand { get; }

        public DelegateCommand<string> AlarmCommand { get; }


        private void OpenDetail(string deviceId)
        {
            // 创建一个导航参数包
            NavigationParameters parameters = new NavigationParameters();
            // 往参数包里面放数据
            parameters.Add("DeviceId", deviceId);
            // 导航时把参数包一起带过去
            _regionManager.RequestNavigate("MainRegion", "DeviceDetailView", parameters);
        }


        private void SendAlarm(string deviceName)
        {
            string msg = $"{deviceName}温度过高";

            //_eventAggregator: 找到消息中心; GetEvent<AlarmEvent>(): 我要“报警事件”这个频道;  Publish(message):往这个频道发布一条消息
            _eventAggregator.GetEvent<AlarmEvent>().Publish(msg);
        }

    }
}
