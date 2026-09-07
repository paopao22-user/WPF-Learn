using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismLineMonitor.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismLineMonitor.ViewModels
{
    //INavigationAware: 这个 ViewModel 想知道自己什么时候被导航进来、什么时候被导航出去。
    public class AlarmViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<string> AlarmList { get; } = new ObservableCollection<string>();

        //1.属性
        private string _deviceName = "";

        public string DeviceName
        {
            get => _deviceName;

            set => SetProperty(ref _deviceName, value);
        }

        private int _alarmCount;

        public int AlarmCount
        {
            get => _alarmCount;

            set => SetProperty(ref _alarmCount, value);
        }

        //构造函数
        public AlarmViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<AlarmEvent>().Subscribe(ReceiveAlarm); //AlarmViewModel 开始监听 `AlarmEvent`。
                                                                             //以后只要有人 Publish AlarmEvent，就调用我的 `ReceiveAlarm()`
        }

        private void ReceiveAlarm(string message)
        {
            AlarmList.Add(message); 

            AlarmCount = AlarmList.Count;
        }

        /// <summary>
        /// Prism 问：原来的这个页面对象还能不能继续复用？
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// 我要被导航离开了
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // 离开时的清理逻辑（如暂无需求，保持方法体为空即可）
        }

        /// <summary>
        /// 我被导航进来了
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //GetValue<string>("DeviceName"):  从导航包里把名字叫 `DeviceName` 的数据拿出来
            // 从参数包提取上一个页面送过来的数据
            DeviceName = navigationContext.Parameters.GetValue<string>("DeviceName");

            AlarmCount = navigationContext.Parameters.GetValue<int>("AlarmCount");
        }
    }
}
