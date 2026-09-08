using Prism.Commands;
using Prism.Mvvm;
using PrismDeviceTableDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PrismDeviceTableDemo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly List<DeviceModel> _allDevices;     //保存原始数据

        //DataGrid 当前显示的数据
        public ObservableCollection<DeviceModel> Devices { get;  } = new ObservableCollection<DeviceModel>();

        //ComboBox：设备类型
        public ObservableCollection<string> DeviceTypes { get; } = new ObservableCollection<string>
        {
            "全部",
            "PLC",
            "温控器"
        };

        //ComboBox：设备状态
        public ObservableCollection<string> StatusOptions { get; } = new ObservableCollection<string>
        {
            "全部",
            "在线",
            "离线"
        };

        //      属性
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        // ComboBox 当前设备类型
        private string _selectedDeviceType = "全部";

        public string SelectedDeviceType
        {
            get { return _selectedDeviceType = "全部"; }
            set 
            { 
                if(SetProperty(ref _selectedDeviceType, value))
                {
                    FilterDevices();
                }
            }
        }

        // ComboBox 当前状态
        private string _selectedStatus = "全部";

        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set 
            { 
                if(SetProperty(ref _selectedStatus, value))
                {
                    FilterDevices();
                }
            }
        }


        // DataGrid 当前选中行
        private DeviceModel _selectedDevice;

        public DeviceModel SelectedDevice
        {
            get { return _selectedDevice; }
            set { 
                    if (SetProperty(ref _selectedDevice, value)) 
                    { 
                        if(value != null)
                        {
                        //将原对象的字符串属性逐个提取并赋值给 ViewModel 中的编辑属性
                            EditDeviceName = value.DeviceName;

                            EditDeviceType = value.DeviceType;

                            EditIpAddress = value.IpAddress;

                            EditStatus = value.Status;

                        }
                    } 
                }
        }

        private string _editDeviceName = "";

        public string EditDeviceName
        {
            get { return _editDeviceName = ""; }
            set { SetProperty(ref _editDeviceName, value); }
        }


        private string _editDeviceType = "PLC";

        public string EditDeviceType
        {
            get => _editDeviceType;

            set => SetProperty(ref _editDeviceType, value);
        }


        private string _editIpAddress = "";

        public string EditIpAddress
        {
            get => _editIpAddress;

            set => SetProperty(ref _editIpAddress, value);
        }

        private string _editStatus = "在线";

        public string EditStatus
        {
            get => _editStatus;

            set => SetProperty(ref _editStatus, value);
        }

        private string _searchKeyword = "";

        public string SearchKeyword
        {
            get => _searchKeyword;

            set => SetProperty(ref _searchKeyword, value);
        }


        //构造函数
        public MainWindowViewModel()
        {
            _allDevices = new List<DeviceModel>
            {
                new DeviceModel
                    {
                        Id = 1,
                        DeviceName = "PLC-01",
                        DeviceType = "PLC",
                        IpAddress = "192.168.1.10",
                        Status = "在线"
                    },

                new DeviceModel
                {
                    Id = 2,
                    DeviceName = "PLC-02",
                    DeviceType = "PLC",
                    IpAddress = "192.168.1.11",
                    Status = "离线"
                },

                new DeviceModel
                {
                    Id = 3,
                    DeviceName = "TEMP-01",
                    DeviceType = "温控器",
                    IpAddress = "192.168.1.20",
                    Status = "在线"
                },

                new DeviceModel
                {
                    Id = 4,
                    DeviceName = "TEMP-02",
                    DeviceType = "温控器",
                    IpAddress = "192.168.1.21",
                    Status = "离线"
                }
            };

            foreach(var device in _allDevices)
            {
                Devices.Add(device);
            }


            AddCommand = new DelegateCommand(AddDevice);

            UpdateCommand = new DelegateCommand(UpdateDevice);

            DeleteCommand = new DelegateCommand(DeleteDevice);

            SearchCommand = new DelegateCommand(FilterDevices);
                
        }


        // 声明命令
        public DelegateCommand AddCommand { get; }

        public DelegateCommand UpdateCommand { get; }

        public DelegateCommand DeleteCommand { get; }

        public DelegateCommand SearchCommand { get; }


        /// <summary>
        /// 过滤设备
        /// </summary>
        private void FilterDevices()
        {
            var result = _allDevices.AsEnumerable(); //AsEnumerable（转为可枚举序列方法）：将其显式向上转型为 IEnumerable<DeviceModel> 接口类型,可使用linq语法

            // 关键字查询
            if (!string.IsNullOrWhiteSpace(SearchKeyword))
            {
                //StringComparison.OrdinalIgnoreCase（忽略大小写的序号字符串比对枚举）
                result = result.Where(x => x.DeviceName.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase) ||
                                        x.IpAddress.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase));
            }

            // 按设备类型筛选
            if (SelectedDeviceType != "全部")
            {
                result = result.Where(x => x.DeviceType == SelectedDeviceType); //筛选选中的设备类型：第一层筛选
            }

            // 按设备状态筛选
            if(SelectedStatus != "全部")
            {
                result = result.Where(x => x.Status == SelectedStatus); //筛查选中的状态:第二层筛选
            }

            // 清空当前 DataGrid 数据
            Devices.Clear();

            // 把筛选结果重新放进去
            foreach(var device in result)
            {
                Devices.Add(device);
            }
        }

        // 新增的本质： 创建一个新对象，然后塞进集合。
        private void AddDevice()
        {
            // 1. 数据合法性前置防御
            if (string.IsNullOrWhiteSpace(EditDeviceName))
            {
                return;
            }

            // 2. 动态自增主键生成
            int newId = _allDevices.Count == 0 ? 1 : _allDevices.Max(x => x.Id) + 1;

            // 3. 构建实体对象
            DeviceModel newDevice = new DeviceModel
            {
                Id = newId,

                DeviceName = EditDeviceName,

                DeviceType = EditDeviceType,

                IpAddress = EditIpAddress,

                Status = EditStatus
            };

            // 4. 存入母库
            _allDevices.Add(newDevice);

            // 5. 重新执行筛选管道
            FilterDevices();

            // 6. 清空输入表单
            ClearEditForm();


        }

        // 更新设备
        private void UpdateDevice()
        {
            if (SelectedDevice == null)
                return;

            SelectedDevice.DeviceName = EditDeviceName;

            SelectedDevice.DeviceType = EditDeviceType;

            SelectedDevice.IpAddress = EditIpAddress;

            SelectedDevice.Status = EditStatus;


            FilterDevices();
        }


        private void DeleteDevice()
        {
            // 1. 防御性空值检查
            if (SelectedDevice == null)
                return;

            // 2. 从全量母库中移除
            _allDevices.Remove(SelectedDevice);

            // 3. 从前台展示集合datagrid中移除
            Devices.Remove(SelectedDevice);

            // 4. 清空选中引用
            SelectedDevice = null;

            // 5. 复位清空输入表单
            ClearEditForm();

            // 6. 重新执行筛选管道
            FilterDevices();
        }


        private void ClearEditForm()
        {
            EditDeviceName = "";

            EditDeviceType = "PLC";

            EditIpAddress = "";

            EditStatus = "在线";
        }
    }
}
