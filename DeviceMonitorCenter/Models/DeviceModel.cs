using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitorCenter.Models
{
    public class DeviceModel
    {

        public int Id { get; set; }

        public string DeviceName { get; set; } = string.Empty;

        public string IpAddress { get; set; } = string.Empty;

        public int Port { get; set; }

        public double Temperature { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
