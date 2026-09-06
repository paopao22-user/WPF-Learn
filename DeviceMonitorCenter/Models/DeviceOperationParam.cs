using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitorCenter.Models
{
    public class DeviceOperationParam
    {

        // 当前要操作的设备
        public DeviceModel? Device { get; set; }

        // 操作类型
        public string OperationType { get; set; } = string.Empty;

        // 指令参数
        public string CommandValue { get; set; } = string.Empty;
    }
}
