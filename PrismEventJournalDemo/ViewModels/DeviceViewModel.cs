using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using PrismEventJournalDemo.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PrismEventJournalDemo.ViewModels
{
    public class DeviceViewModel:BindableBase
    {
        //声明接口和命令
        private readonly IEventAggregator _eventAggregator;

        public DelegateCommand<string> AlarmCommand { get; }

        //构造函数注入IEventAggregator
        public DeviceViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            AlarmCommand = new DelegateCommand<string>(SendAlarm);
        }

        private void SendAlarm(string deviceName)
        {
            string msg = $"{deviceName}温度过高";

            _eventAggregator.GetEvent<AlarmEvent>().Publish(msg);       //发布事件
        }
    }
}
