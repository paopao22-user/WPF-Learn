using DeviceMonitorCenter.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DeviceMonitorCenter.Converters
{
    public class DeviceOperationConverter : IMultiValueConverter
    {
        // 核心步骤：将输入的多个值打包为一个数组对象
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DeviceOperationParam param = new DeviceOperationParam();

            // 第1个值：当前设备
            param.Device = values[0] as DeviceModel;

            // 第2个值：操作类型
            param.OperationType = values[1]?.ToString() ?? string.Empty;

            // 第3个值：指令参数
            param.CommandValue = values[2]?.ToString() ?? string.Empty;

            return param;
        }

        // 命令传参属于单向传递，逆向转换直接抛出异常即可
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
