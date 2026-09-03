using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte字节流练习_压力传感器
{
    public class SerialPortService
    {

        private SerialPort? _serialPort;

        public SerialPortService()
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = "COM1";
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Parity = Parity.None;

            // 注册接收事件
            _serialPort.DataReceived += _serialPort_DataReceived;
        }

        /// <summary>
        /// 打开
        /// </summary>
        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
                Console.WriteLine("COM1打开成功");
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                Console.WriteLine("COM1已关闭");
            }
        }

        /// <summary>
        /// 发送byte[] 数据
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data)   
        {
            //假如没有打开连接就返回
            if (!_serialPort.IsOpen)
            {
                Console.WriteLine("串口没有打开");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("【准备发送】");

            Console.WriteLine($"发送字节数量:{data.Length}");
            Console.WriteLine($"发送Hex:{ProtocolHelper.ByteArrayToHex(data)}");

            //从 data 数组的第 0 个格子开始，连续搬出 data.Length 个字节，推上串口发货小推车！
            //_serialPort.Write(data, 0, count): 把 C# 数组的字节 ➜ 复制进驱动发送管道
            //_serialPort.Read(buffer, 0, count): 把驱动接收管道里的字节 ➜ 倒进 C# 数组
            _serialPort.Write(data, 0, data.Length);    // 调用底层 Win32 WriteFile 驱动接口，将内存中的字节流一口气灌入 Windows 内核的串口发送缓冲区中

            Console.WriteLine("发送成功");
        }

        /// <summary>
        /// COM1 收到数据
        /// </summary>
        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //学习阶段简单等待一下
                Thread.Sleep(20);

                // 查看当前缓冲区多少 Byte
                int count = _serialPort.BytesToRead;

                // 如果没有读取到数据就返回
                if (count <= 0)
                    return;

                // 创建接收数组
                byte[] buffer = new byte[count];    //先创建空的字节数组

                // 从串口缓冲区读取数据
                int actualRead = _serialPort.Read(buffer, 0, count);

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("【收到数据】");

                Console.WriteLine($"收到Byte数量: {actualRead}");
                Console.WriteLine($"收到Hex:{ProtocolHelper.ByteArrayToHex(buffer)}");

                Console.WriteLine();
                Console.WriteLine("逐 Byte 查看：");

                for(int i = 0; i < buffer.Length; i++)
                {
                    Console.WriteLine($"buffer[{i}] = {buffer[i]} = 0x{buffer[i]:X2}");
                }

                // 交给协议层解析
                if(buffer.Length >= 7)
                {
                    // 解析压力响应
                    double pressure = ProtocolHelper.ParsePressure(buffer);
                    Console.WriteLine();

                    Console.WriteLine($"最终压力：{pressure} bar");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"接收错误:{ex.Message}");
            }
        }


    }
}
