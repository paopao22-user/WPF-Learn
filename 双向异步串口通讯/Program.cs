// See https://aka.ms/new-console-template for more information
using System;
using System.IO.Ports;
using System.Threading;

namespace SerialPortTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("    阶段一：C# 上位机字符串双向异步收发实战      ");
            Console.WriteLine("==================================================");

            // 1. 实例化 SerialPort (COM1, 9600波特率, 无校验, 8数据位, 1停止位)
            using (SerialPort serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One))
            {
                try
                {
                    // 2. 关键核心：在打开串口前，订阅数据到达事件 (注册回调函数)
                    serialPort.DataReceived += SerialPort_DataReceived;

                    // 3. 打开 COM1 端口
                    serialPort.Open();
                    Console.WriteLine(">>> [COM1] 端口打开成功！全双工异步监听已就绪。");
                    Console.WriteLine(">>> 提示：现在你随时可以在 COM2 窗口中给 C# 发数据！\n");

                    // 4. 主线程模拟定时发送心跳指令 (发送 3 条)
                    for (int i = 1; i <= 3; i++)
                    {
                        string sendMsg = $"[C# 发送] 请求第 {i} 次设备自检握手";
                        serialPort.WriteLine(sendMsg);
                        Console.WriteLine($"[我方主动发出] -> {sendMsg}");

                        // 延时 2 秒，期间如果对方回传消息，会立即穿插打印出来
                        Thread.Sleep(2000);
                    }

                    Console.WriteLine("\n--------------------------------------------------");
                    Console.WriteLine(">>> 主线程发送完毕，当前进入【持续监听模式】。");
                    Console.WriteLine(">>> 请在旁边的 COM2 串口助手白框中敲字或点击发送！");
                    Console.WriteLine(">>> (按键盘回车键 Enter 退出程序)");
                    Console.WriteLine("--------------------------------------------------");

                    // 5. 阻止主线程退出，保持控制台存活，确保后台监听不中断
                    Console.ReadLine(); //在控制台应用中至关重要。控制台程序如果执行完 `Main` 函数就会自动闪退；用这行代码卡住主线程，让程序保持挂起运行，后台线程池才能一直监听接收。
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ 通讯异常: {ex.Message}");
                }
                finally
                {
                    // 6. 良好编程素养：退出前反注册事件
                    serialPort.DataReceived -= SerialPort_DataReceived;
                }
            } // using 结束自动调用 Close() 释放端口

            Console.WriteLine("程序已退出，端口已安全释放。");
        }

        /// <summary>
        /// 串口接收事件回调方法：一旦有数据到达，底层线程池会自动调用此方法
        /// </summary>
        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // 将事件源转换为 SerialPort 对象
            SerialPort sp = (SerialPort)sender;

            try
            {
                // 一次性读出当前接收缓冲区中的所有可用字符串
                string receivedText = sp.ReadExisting();

                // 过滤掉可能存在的空信号
                if (!string.IsNullOrEmpty(receivedText))
                {
                    // 使用绿色字体高亮打印收到的下位机内容，形成鲜明视觉对比
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[⚡ 收到 COM2 下位机回复] -> {receivedText.Trim()}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ 读取异常: {ex.Message}");
            }
        }
    }
}