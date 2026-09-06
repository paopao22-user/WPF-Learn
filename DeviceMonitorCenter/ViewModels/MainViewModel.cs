using DeviceMonitorCenter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitorCenter.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        // 声明并实例化一个可观察集合
        //它就是一个**“自带喇叭广播”的增强版列表**。它在底层内部包装了一个传统的 List<T>，并在每一次发生数据增删改时，自动向外界发出广播通知。
        public ObservableCollection<DeviceModel> Devices { get; } = new ObservableCollection<DeviceModel>();

        //构造函数
        public MainViewModel()
        {
            Devices.Add(new DeviceModel
            {
                Id = 1,
                DeviceName = "温度采集器01",
                IpAddress = "192.168.1.10",
                Port = 502,
                Temperature = 25.6,
                Status = "在线"
            });

            Devices.Add(new DeviceModel
            {
                Id = 2,
                DeviceName = "PLC-01",
                IpAddress = "192.168.1.20",
                Port = 102,
                Temperature = 32.1,
                Status = "离线"
            });

            Devices.Add(new DeviceModel
            {
                Id = 3,
                DeviceName = "压力仪表01",
                IpAddress = "192.168.1.30",
                Port = 502,
                Temperature = 28.4,
                Status = "在线"
            });

            //构造函数初始化
            ConnectDeviceCommand = new RelayCommandOfT<DeviceModel>(ConnectDevice);

            SendOperationCommand = new RelayCommandOfT<DeviceOperationParam>(SendDeviceOperation);
        }


        //声明命令
        public RelayCommandOfT<DeviceModel> ConnectDeviceCommand { get; }

        public RelayCommandOfT<DeviceOperationParam> SendOperationCommand { get; }


        //声明属性
        private string _statusText = "等待操作";

        public string StatusText
        {
            get => _statusText;

            set => SetProperty(ref _statusText, value);
        }

        private DeviceModel? _selectedDevice;

        public DeviceModel? SelectedDevice
        {
            get => _selectedDevice;

            set => SetProperty(ref _selectedDevice, value);

        }

        private string? _operationType = "读取温度";

        public string OperationType
        {
            get => _operationType;

            set => SetProperty(ref _operationType, value);
        }

        private string? _commandValue = "022";

        public string? CommandValue
        {
            get => _commandValue;

            set => SetProperty(ref _commandValue, value);
        }


        //业务
        private void ConnectDevice(DeviceModel device)
        {
            StatusText = $"正在连接:{device.DeviceName}" +
                        $"{device.IpAddress}:{device.Port}";
        }

        private void SendDeviceOperation(DeviceOperationParam param)
        {
            if (param.Device == null)
            {
                StatusText = "请先选择设备";
                return;
            }

            StatusText = $"设备: {param.Device.DeviceName}," +
                        $"操作:{param.OperationType}," +
                        $"参数:{param.CommandValue}";
        }

    }
}
