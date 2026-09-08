using Prism.Events;
using Prism.Mvvm;
using PrismEventJournalDemo.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismEventJournalDemo.ViewModels
{
    public class AlarmViewModel:BindableBase
    {
        //声明接口和命令
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<string> AlarmList { get; } = new ObservableCollection<string>();


        //声明属性
        private int _alarmCount;

        public int AlarmCount
        {
            get => _alarmCount;

            set => SetProperty(ref _alarmCount, value);
        }

        //IEventAggregator注入构造函数, 接收AlarmEvent 事件
        public AlarmViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<AlarmEvent>().Subscribe(ReceiveAlarm);
        }


        private void ReceiveAlarm(string msg)
        {
            AlarmList.Add(msg);

            AlarmCount = AlarmList.Count;
        }
    }
}
