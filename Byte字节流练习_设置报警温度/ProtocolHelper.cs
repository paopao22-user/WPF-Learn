using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte字节流练习_设置报警温度
{
    public static class ProtocolHelper
    {
        /// <summary>
        /// 构建设置报警温度请求 : 按照协议组包
        /// 按照给出的温度值，拆成高字节和低字节，组装成完整的请求报文
        /// </summary>
        /// <param name="temperature"></param>
        /// <returns></returns>
        public static byte[] BuildSetAlarmRequest(double temperature)
        {
            // 1. 实际温度放大10倍
            int rawValue  = (int)Math.Round(temperature * 10);

            //// 2. 检查16位无符号整数范围
            if(rawValue < 0 || rawValue > 25536)
            {
                throw new Exception("温度数据超出协议允许范围");
            }

            //3.获取高字节
            byte highByte = (byte)((rawValue >> 8) & 0xFF);

            //4. 获取低字节
            byte lowByte = (byte)(rawValue & 0xFF);

            Console.WriteLine();
            Console.WriteLine("【开始组包】");

            Console.WriteLine(
                $"输入温度：{temperature}℃");

            Console.WriteLine(
                $"放大10倍：{rawValue}");

            Console.WriteLine(
                $"16进制：0x{rawValue:X4}");

            Console.WriteLine(
                $"高字节：0x{highByte:X2}");

            Console.WriteLine(
                $"低字节：0x{lowByte:X2}");

            // 5. 按照协议组包
            byte[] request =
            {
                0xAC,   // 帧头
                0x01,   // 设备地址
                0x10,   // 设置报警温度
                0x02,   // 数据长度
                highByte,   // 温度高字节
                lowByte     // 温度低字节
            };

            //6. 返回
            return request;
        }

        /// <summary>
        /// Byte数组转换成Hex显示
        /// </summary>
        public static string ByteArrayToHex(byte[] data)
        {
            string result = "";

            foreach(byte item in data)
            {
                result += item.ToString("X2") + " ";
            }

            return result.Trim();
        }

        /// <summary>
        /// 解析设备设置结果
        ///
        /// 成功：
        /// AC 01 10 01 00
        ///
        /// 失败：
        /// AC 01 10 01 01
        /// </summary>
        public static bool ParseSetAlarmResponse(byte[] response)
        {
            if(response == null || response.Length < 5)
            {
                throw new Exception("响应数据长度不足");
            }

            if (response[0] != 0xAC)
            {
                throw new Exception("帧头错误");
            }

            if (response[1] != 0x01)
            {
                throw new Exception("设备地址错误");
            }

            if (response[2] != 0x10)
            {
                throw new Exception("功能码错误");
            }

            if (response[3] != 0x01)
            {
                throw new Exception("数据长度错误");
            }

            byte status = response[4];

            if(status == 0x00)
            {
                Console.WriteLine("设备返回：设置成功");
                return true;
            }

            else
            {
                Console.WriteLine($"设备返回：设置失败，状态码 0x{status:X2}");
                return false;
            }
        }
    }
}
