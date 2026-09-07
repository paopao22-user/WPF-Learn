using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismLineMonitor.ViewModels
{
    public class DeviceDetailViewModel : BindableBase, INavigationAware
    {
        //属性
        private string _deviceId = "";

        public string DeviceId
        {
            get => _deviceId;
            set => SetProperty(ref _deviceId, value);
        }


        private string _deviceName = "";

        public string DeviceName
        {
            get => _deviceName;

            set => SetProperty(ref _deviceName, value);
        }

        private string _ipAddress = "";

        public string IpAddress
        {
            get => _ipAddress;

            set => SetProperty(ref _ipAddress, value);
        }




        /// <summary>
        /// 能否复用原来的这个页面对象
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// 我被导航离开了
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        /// <summary>
        /// 我被导航进来了
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 从参数包提取上一个页面送过来的数据
            DeviceId = navigationContext.Parameters.GetValue<string>("DeviceId");

            if(DeviceId == "1")
            {
                DeviceName = "PLC-01";

                IpAddress = "192.168.1.10";
            }

            else if(DeviceId == "2")
            {
                DeviceName = "PLC-02";

                IpAddress = "192.168.1.11";
            }

            else if(DeviceId == "3")
            {
                DeviceName = "PLC-03";

                IpAddress = "192.168.1.12";
            }
        }
    }
}
