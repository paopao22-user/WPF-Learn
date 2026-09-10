using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismDeviceTableDemo.Models
{
    public static class DeviceOptions
    {

        public static List<string> StatusOptions { get; } = new List<string>
        {
            "在线",
            "离线",
            "维护"
        };

        public static List<string> DeviceTypes { get; } = new List<string>
        {
            "PLC",
            "温控器",
            "传感器"
        };
    }
}
