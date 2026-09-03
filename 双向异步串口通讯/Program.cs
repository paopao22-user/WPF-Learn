// See https://aka.ms/new-console-template for more information
using System;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSerialPortDemo
{
    internal class Program
    {
        // 1. 现代 C# 的 async 异步主入口
        static async Task Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("     现代 C# 风格：基于 async/await 的串口异步读取 ");
            Console.WriteLine("==================================================");

            using (SerialPort serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One))
            {
                //开启串口通讯
                serialPort.Open();
                Console.WriteLine(">>> COM1 已经打开，开启异步监听任务...\n");

                // 2. 启动一个独立的后台异步监听长任务 (不阻塞主线程)
                _ = ReceiveDataAsync(serialPort);

                // 3. 主线程可以继续做自己的事，比如每隔 3 秒主动发一条问候
                for (int i = 1; i <= 5; i++)
                {
                    serialPort.WriteLine($"[主线程心跳] #{i}");
                    Console.WriteLine($"[主线程报告] 正在执行其他业务... {i}");

                    // 现代异步等待：让出线程，绝不卡死
                    await Task.Delay(3000);
                }

                Console.WriteLine("\n按回车键退出程序...");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 纯正的现代 async/await 异步接收循环方法
        /// </summary>
        private static async Task ReceiveDataAsync(SerialPort sp)
        {
            byte[] buffer = new byte[1024]; // 1 KB 缓冲区

            while (sp.IsOpen)   // 只要串口还开着，就一直循环监听
            {
                try
                {
                    // 4. 标志性的 await 异步非阻塞等待：
                    // 当没有数据时，这行代码会挂起让出 CPU，绝不卡死任何线程！
                    // 一旦 COM2 发来数据，操作系统立刻唤醒并恢复往下执行！
                    int bytesRead = await sp.BaseStream.ReadAsync(buffer, 0, buffer.Length);

                    // 5. 如果读取到数据，就把字节流解码为字符串并打印
                    if (bytesRead > 0)
                    {
                        string message = Encoding.Default.GetString(buffer, 0, bytesRead);  // 默认编码解码
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"\n[⚡ await 异步读取成功] -> {message.Trim()}"); // 
                        Console.ResetColor(); // 恢复默认控制台颜色
                    }
                }
                catch (Exception)
                {
                    break; // 端口关闭时退出循环
                }
            }
        }
    }
}