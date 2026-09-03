// See https://aka.ms/new-console-template for more information
using System;
using System.IO.Ports; // 必须引用串口命名空间
using System.Threading;

namespace SerialPortTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("        C# 上位机极简串口通讯实战测试              ");
            Console.WriteLine("==================================================");

            // 1. 实例化 SerialPort 类 (参数: 端口号, 波特率, 校验位, 数据位, 停止位)
            using (SerialPort serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One))
            {
                try
                {
                    // 2. 打开 COM1 端口
                    serialPort.Open();
                    Console.WriteLine(">>> [COM1] 端口打开成功！开始向 COM2 发送工业数据...\n");

                    // 3. 循环发送 5 条模拟指令，每秒发一条
                    for (int i = 1; i <= 5; i++)
                    {
                        // 构造模拟工业温度报文
                        string message = $"[C# 上位机] 周期上报第 {i} 组数据: 电机温度 36.{i} ℃";

                        // 4. 调用 WriteLine 将字符串打包为字节流发送
                        serialPort.WriteLine(message);

                        Console.WriteLine($"已发出 -> {message}");

                        // 延时 1000 毫秒，方便肉眼观察
                        Thread.Sleep(1000);
                    }

                    Console.WriteLine("\n>>> 全部 5 条数据发送完成！请看旁边的 COM2 窗口接收区！");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ 通讯失败: {ex.Message}");
                    Console.WriteLine("排查建议: 请确认刚才的 COM1 串口助手已经关闭！");
                }
            } // 5. using 语句块结束时，会自动调用 serialPort.Close() 优雅释放端口资源

            Console.WriteLine("\n按任意键退出程序...");
            Console.ReadKey();
        }
    }
}