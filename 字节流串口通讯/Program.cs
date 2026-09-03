// See https://aka.ms/new-console-template for more information
using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace SerialPortHexDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("    阶段二：C# 原生字节流 (Hex Byte) 收发与解析实战 ");
            Console.WriteLine("==================================================");

            using (SerialPort serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One))
            {
                try
                {
                    // 1. 绑定原生字节到达事件
                    serialPort.DataReceived += SerialPort_DataReceived;

                    // 2. 打开 COM1
                    serialPort.Open();
                    Console.WriteLine(">>> [COM1] 端口打开成功！工业二进制监控已就绪。\n");

                    // 3. 构造一条标准工业 Modbus 查询报文：读取 1 号从站的保持寄存器
                    // 报文结构: 站号(01) + 功能码(03) + 起始地址(00 00) + 寄存器数(00 01) + CRC校验码(84 0A)
                    byte[] queryFrame = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x01, 0x84, 0x0A };

                    // 4. 发送 3 次查询报文
                    for (int i = 1; i <= 3; i++)
                    {
                        // 彻底告别 WriteLine，使用 Write 直接发送纯二进制字节数组
                        serialPort.Write(queryFrame, 0, queryFrame.Length);

                        // 将发出的字节以友好的 Hex 格式打印在控制台
                        string hexStr = string.Join(" ", queryFrame.Select(b => b.ToString("X2")));
                        Console.WriteLine($"[我方发出 Hex 报文 #{i}] -> {hexStr}");

                        Thread.Sleep(3000); // 每隔 3 秒发一次请求
                    }

                    Console.WriteLine("\n--------------------------------------------------");
                    Console.WriteLine(">>> 请在旁边的 COM2 串口助手中，用【Hex 模式】回复数据！");
                    Console.WriteLine(">>> 推荐在 COM2 输入并发送这串标准数据：");
                    Console.WriteLine("    01 03 02 01 70 B8 44");
                    Console.WriteLine("    (其中 01 70 代表十六进制温度值 368，即 36.8 ℃)");
                    Console.WriteLine("--------------------------------------------------");

                    Console.ReadLine(); // 阻塞保持监听
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ 异常: {ex.Message}");
                }
                finally
                {
                    serialPort.DataReceived -= SerialPort_DataReceived;
                }
            }
        }

        /// <summary>
        /// 接收原生字节并完成数值换算
        /// </summary>
        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            try
            {
                // 等待 20 毫秒，确保网线中的微量延迟把后续字节全部吐入缓冲区
                Thread.Sleep(20);

                // 1. 获取当前缓冲区积攒的字节总数
                int bytesToRead = sp.BytesToRead;
                if (bytesToRead == 0) return;

                // 2. 声明精准尺寸的字节数组，一次性读出
                byte[] recvBuffer = new byte[bytesToRead];
                sp.Read(recvBuffer, 0, bytesToRead);

                // 3. 将收到的字节以 Hex 打印
                string hexReceived = string.Join(" ", recvBuffer.Select(b => b.ToString("X2")));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n[⚡ 收到 COM2 原始 Hex 帧] -> {hexReceived}");
                Console.ResetColor();

                // 4. 工业报文简易解析演示 (假设收到标准 7 字节响应: 01 03 02 [高字节] [低字节] CRC1 CRC2)
                if (recvBuffer.Length >= 5 && recvBuffer[0] == 0x01 && recvBuffer[1] == 0x03)
                {
                    // 截取第 4、5 两个字节（索引 3 和 4）
                    byte highByte = recvBuffer[3];
                    byte lowByte = recvBuffer[4];

                    // 大端拼接还原为整数
                    int rawValue = (highByte << 8) | lowByte;

                    // 转换为实际工程量（除以 10.0）
                    double temperature = rawValue / 10.0;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[🎉 报文解析成功] 设备地址: {recvBuffer[0]}, 原始数字: {rawValue}, 换算后实际温度: {temperature:F1} ℃");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ 解析报错: {ex.Message}");
            }
        }
    }
}
