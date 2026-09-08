using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismDeviceTableDemo.Models
{
    public class DeviceModel:BindableBase
    {

        private int _id;

        public int Id
        {
            get => _id;

            set => SetProperty(ref _id, value);
        }

        private string _deviceName = "";

        public string DeviceName
        {
            get => _deviceName;

            set => SetProperty(ref _deviceName, value);
        }

        private string _deviceType = "";

        public string DeviceType
        {
            get => _deviceType;

            set => SetProperty(ref _deviceType, value);
        }

        private string _ipAddress = "";

        public string IpAddress
        {
            get => _ipAddress;

            set => SetProperty(ref _ipAddress, value);
        }

        private string _status = "";

        public string Status
        {
            get => _status;

            set => SetProperty(ref _status, value);
        }
    }
}
