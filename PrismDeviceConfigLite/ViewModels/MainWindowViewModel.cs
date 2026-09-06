using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismDeviceConfigLite.ViewModels
{
    public class MainWindowViewModel:BindableBase
    {

        //1.属性
        private string? _deviceName = "PLC";

        public string? DeviceName
        {
            get => _deviceName;

            set => SetProperty(ref _deviceName, value);
        }

        private string? _ipAddress = "155.26.1";

        public string? IpAddress
        {
            get => _ipAddress;

            set => SetProperty(ref _ipAddress, value);
        }

        private int _port = 22;

        public int Port
        {
            get => _port;

            set => SetProperty(ref _port, value);
        }

        private string? _statusMessage = "状态显示中...";

        public string? StatusMessage
        {
            get => _statusMessage;

            set => SetProperty(ref _statusMessage, value);
        }


        //2.构造函数
        public MainWindowViewModel()
        {
            SaveCommand = new DelegateCommand(SaveConfig);

            ResetCommand = new DelegateCommand(ResetDefault);
        }


        //3.声明命令
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand ResetCommand { get; }


        //4.执行命令业务
        public void SaveConfig()
        {
            StatusMessage = $"配置已保存: {DeviceName} ({IpAddress}:{Port}) [{DateTime.Now:HH:mm:ss}]";
        }

        public void ResetDefault()
        {
            DeviceName = "PLC";
            IpAddress = "155.26.1";
            Port = 22;
            StatusMessage = "已恢复初始默认值";
        }

    }
}
