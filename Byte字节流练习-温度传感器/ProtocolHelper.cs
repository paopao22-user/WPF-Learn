using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte字节流练习_温度传感器
{
    public static class ProtocolHelper
    {
        /// <summary>
        /// 构建“读取温度”请求帧
        /// AA 01 03 00
        /// </summary>
        public static byte[] BuildReadTemperatureRequest()
        {
            byte[] request =
            {
            0xAA,   // 帧头
            0x01,   // 设备地址
            0x03,   // 功能码：读取温度
            0x00    // 保留位
        };

            return request;
        }


        /// <summary>
        /// 把 byte[] 转成 Hex 字符串显示
        /// 例如：
        /// AA 01 03 00
        /// </summary>
        public static string ByteArrayToHex(byte[] data)
        {
            string result = "";     // 1. 声明并初始化一个空字符串，作为累加容器

            // 2. 遍历字节数组中的每一个 byte 元素
            foreach (byte item in data)
            {
                // 3. 核心转换与拼接：
                // item.ToString("X2")：将当前字节转为 2 位大写十六进制（如 10 -> "0A"）
                // + " "：在每个字节后面追加一个空格，方便人眼将字节彼此分开
                // result +=：把新生成的片段追加到大字符串末尾
                result += item.ToString("X2") + "";
            }

            // 4. 修剪并返回结果
            // Trim() 会自动移除字符串最前端和最末尾的空白字符
            return result.Trim();
        }


        /// <summary>
        /// 工业温度报文专用解析器：将 6 字节的二进制报文拆解还原为实际摄氏度浮点数
        /// 报文协议规范：[帧头(AA)] [从站(01)] [功能码(03)] [数据长(02)] [温度高字节] [温度低字节]
        /// </summary>
        /// <param name="response">下位机回传的原始字节数组</param>
        /// <returns>返回真实温度浮点数 (单位: ℃)</returns>
        public static double ParseTemperature(byte[] response)
        {
            // 1. 防御关卡一：长度截断安全检查，若小于 6 字节则拒绝访问后续下标，杜绝数组越界
            if (response == null || response.Length < 6)
            {
                throw new Exception("响应数据长度不足");
            }

            // 2. 防御关卡二：帧头同步字校验，检查第 0 字节是否为约定的 0xAA，过滤网线随机电磁杂波
            if (response[0] != 0xAA)
            {
                throw new Exception("帧头错误");
            }

            // 3. 防御关卡三：设备地址核对，确认当前应答是否来自我们指定的 1 号从站，防止串台
            if (response[1] != 0x01)
            {
                throw new Exception("设备地址错误");
            }

            // 4. 防御关卡四：功能码核对，确认下位机回应的是否为 03H (读保持寄存器) 指令
            if (response[2] != 0x03)
            {
                throw new Exception("功能码错误");
            }

            // 5. 精准提取高低字节：跳过第 3 位的长度说明符，直接截取第 4 位的高字节和第 5 位的低字节
            byte highByte = response[4]; // 提取温度高 8 位 (权重为 256)
            byte lowByte = response[5];  // 提取温度低 8 位 (权重为 1)

            // 6. 控制台解包推演过程可视化输出
            Console.WriteLine();
            Console.WriteLine("开始解析温度：");
            Console.WriteLine($"highByte = 0x{highByte:X2}"); // 打印高字节的 16 进制形式
            Console.WriteLine($"lowByte  = 0x{lowByte:X2}");  // 打印低字节的 16 进制形式

            // 7. 核心位运算推演一：按位左移 8 位 (<< 8)
            // 本质相当于数学上的 highByte * 256；
            // 把原本住在 1 楼的 8 个二进制位推到 2 楼，在 1 楼腾出 8 个全为 0 的空房间
            int highPart = highByte << 8;

            Console.WriteLine();
            Console.WriteLine($"highByte << 8 = 0x{highPart:X4}"); // 打印移位后生成的 16 位整数

            // 8. 核心位运算推演二：按位或运算 (|)
            // 本质相当于数学上的加号 (+)；
            // 因为 1 楼全是 0，低字节 lowByte 直接无缝落入 1 楼空房间，完成两个单字节的拼合
            int rawValue = (highByte << 8) | lowByte;

            Console.WriteLine($"(highByte << 8) | lowByte = 0x{rawValue:X4}");
            Console.WriteLine($"原始十进制值 = {rawValue}"); // 打印还原出的 16 位整数 (例如 368)

            // 9. 工程量比例缩放转换：
            // 工业设备为节省带宽把温度放大了 10 倍，上位机除以 10.0 还原实际值；
            // 注意：必须除以 10.0 (浮点数字面量) 而非整数 10，强制保留小数位精度，防止被截断为整数！
            double temperature = rawValue / 10.0;

            Console.WriteLine($"实际温度 = {rawValue} / 10.0 = {temperature}℃");

            // 10. 返回最终转换出的真实温度数值
            return temperature;
        }
    }
}
