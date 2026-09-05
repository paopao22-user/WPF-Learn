using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace 设备通讯参数校验面板.ViewModels
{
    public class DeviceConfigViewModel: ViewModelBase
    {
        private string _statusText = "等待配置";

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        // 01. 属性对应的私有字段
        private string _deviceName = string.Empty;

        public string DeviceName
        {
            get => _deviceName;
            set 
            { 
                if(SetProperty(ref _deviceName, value))
                {
                    ConnectCommand.RaiseCanExecuteChanged();
                    ResetCommand.RaiseCanExecuteChanged();
                }
            }

            
        }

        private string _ipAddress = string.Empty;

        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                if (SetProperty(ref _ipAddress, value))
                {
                    ConnectCommand.RaiseCanExecuteChanged();
                    ResetCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _port = string.Empty;
        public string Port
        {
            get => _port;
            set
            {
                if(SetProperty(ref _port, value))
                {
                    ConnectCommand.RaiseCanExecuteChanged();
                    ResetCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _interval = string.Empty;

        public string Interval
        {
            get => _interval;

            set
            {
                if(SetProperty(ref _interval, value))
                {
                    ConnectCommand.RaiseCanExecuteChanged();
                    ResetCommand.RaiseCanExecuteChanged();
                }
            }

        }

        // 声明两个Command
        public RelayCommand ConnectCommand { get; }

        public RelayCommand ResetCommand { get; }

        public ParameterRelayCommand ApplyPresetCommand {  get; }

        public RelayCommand<string> SetIntervalCommand { get; }

        //构造函数
        public DeviceConfigViewModel()
        {
            ConnectCommand = new RelayCommand(execute: Connect, canExecute: CanConnect);
            ResetCommand = new RelayCommand(execute: Reset, canExecute: CanReset);
            ApplyPresetCommand = new ParameterRelayCommand(execute:ApplyPreset);
            SetIntervalCommand = new RelayCommand<string>(execute: SetInterval);
        }

        public void SetInterval(string interval)
        {
            Interval = interval;
            StatusText = $"采样周期已设置为 {interval} ms";
        } 

        private void ApplyPreset(object? parameter)
        {
            string preset = parameter?.ToString() ?? string.Empty;

            if(preset == "PLC")
            {
                DeviceName = "西门子 PLC";
                IpAddress = "192.168.1.10";
                Port = "102";
                Interval = "1000";

                StatusText = "已加载 PLC 通讯参数";
            }

            else if(preset == "ModbusTCP")
            {
                DeviceName = "Modbus TCP设备";
                IpAddress = "192.168.1.20";
                Port = "502";
                Interval = "1000";

                StatusText = "已加载 Modbus TCP 通讯参数";
            }

            else if(preset == "LocalTest")
            {
                DeviceName = "本机测试设备";
                IpAddress = "127.0.0.1";
                Port = "9000";
                Interval = "500";

                StatusText = "已加载本机测试参数";
            }
        }


        private void Connect()
        {
            StatusText = $"已准备连接: {DeviceName}- {IpAddress}:{Port}";
        }

        private bool CanConnect()
        {
            // 1. 设备名称不能为空
            if (string.IsNullOrWhiteSpace(DeviceName))
                return false;

            // 2. IP地址不能为空
            if (string.IsNullOrWhiteSpace(IpAddress))
                return false;

            // 3. 端口必须是数字
            if (!int.TryParse(Port, out int port))
                return false;

            // 4. 端口范围必须正确
            if (port < 1 || port > 65535)
                return false;

            // 5. 采样周期必须是数字
            if (!int.TryParse(Interval, out int interval))
                return false;

            // 6. 采样周期必须大于0
            if (interval <= 0)
                return false;

            return true;
        }

        private void Reset()
        {
            DeviceName = string.Empty;
            IpAddress = string.Empty;
            Port = string.Empty;
            Interval = string.Empty;

            StatusText = "状态已清空";
        }

        private bool CanReset()
        {
            return !string.IsNullOrEmpty(DeviceName) || !string.IsNullOrEmpty(Port)
                || !string.IsNullOrEmpty(IpAddress) || !string.IsNullOrEmpty(Interval);
        }
    }
}
