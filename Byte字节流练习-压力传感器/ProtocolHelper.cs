using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte字节流练习_压力传感器
{
    public static class ProtocolHelper
    {
        /// <summary>
        /// 构建读取压力命令
        /// AB 02 04 00
        /// </summary>
        /// <returns></returns>
        public static byte[] BuildReadPressureRequest()
        {
            byte[] request =
            {
                0xAB,   //帧头
                0x02,   //设备地址
                0x04,   //功能码：读取压力
                0x00    //保留位
            };

            return request;
        }

        /// <summary>
        /// byte[] 转为Hex,仅仅用于显示
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteArrayToHex(byte[] data)
        {
            // 1. 声明并初始化一个字符串变量，作为累加拼接的起始文本容器
            // 注：即便此处包含初始空格 " "，最后的 Trim() 也会将其安全消除
            string result = " ";

            foreach(byte item in data)
            {
                // 2. 核心转换与累加拼接操作：
                // item.ToString("X2")：
                //   - 'X' 表示将当前字节转为大写十六进制 (0~9, A~F)
                //   - '2' 表示强制占用两位，不足两位在前面自动补 0 (例如数值 1 转为 "01"，数值 10 转为 "0A")
                // + " "：在每个十六进制数字后追加一个英文半角空格，让人眼能清晰区分每一个独立字节
                // result +=：将新拼好的单字节片段追加挂载到大字符串末尾
                result += item.ToString("X2") + " ";
            }
            // 3. 清除冗余首尾空格并返回最终结果：
            // Trim() 会扫描整个字符串的最左侧与最右侧，自动切除最前端的初始空格和末尾多余的一个空格，
            // 最终输出两头干净、中间带空格的标准报文字符串
            return result.Trim();
        }

        /// <summary>
        /// 解析压力响应
        /// </summary>
        /// AB 02 04 03 01 04 D2
        /// <param name="response"></param>
        /// <returns></returns>
        public static double ParsePressure(byte[] response)
        {
            //1.长度检测
            if(response.Length < 7 || response == null)
            {
                throw new Exception("长度不足");
            }

            //2.帧头检测
            if (response[0] != 0xAB)
            {
                throw new Exception("帧头错误");
            }

            //3.地址检测
            if (response[1] != 0x02)
            {
                throw new Exception("地址错误");
            }

            //4.功能码检测
            if (response[2] != 0x04)
            {
                throw new Exception("功能码错误");
            }

            //5.数据长度检测
            if (response[3] != 0x03)
            {
                throw new Exception("数据长度错误");
            }

            //6.获取状态
            byte status = response[4];

            //7.获取压力高低字节
            byte highbyte = response[5];
            byte lowbyte = response[6];

            Console.WriteLine();
            Console.WriteLine("------ 开始解析压力 ------");

            Console.WriteLine($"状态 Byte: 0x{status:X2}");
            Console.WriteLine($"高字节 Byte: 0x{highbyte:X2}");
            Console.WriteLine($"低字节 Byte: 0x{lowbyte:X2}");

            // 8. 高低字节拼成16位整数
            int rawPressure = (highbyte << 8) | lowbyte;    //(highbyte << 8): 向左移8位到上一层， lowbyte保持不变， 合并之后位16位整数，并转为int类型

            Console.WriteLine($"原始压力值: 0x{rawPressure:X4}");
            Console.WriteLine($"原始十进制:{rawPressure}");

            // 9. 转换为真实压力
            double pressure = rawPressure / 100.0;

            Console.WriteLine($"真实压力为:{pressure} bar");

            //10. 检查状态 Byte 的最低位
            // &的功能：用来遮住无关位，单挑探测目标位；  1 & 1 = 1，其他情况全是 0（只有两边都是 1，结果才是 1）
            // | 的功能： 用来合并数据，或者强制把某一位打开（置 1);  0 | 0 = 0，只要有一边是 1，结果就是 1
            bool isAlarm = (status & 0x01) != 0;
            Console.WriteLine($"高压报警:{isAlarm}");

            return pressure;
        }
    }
}
