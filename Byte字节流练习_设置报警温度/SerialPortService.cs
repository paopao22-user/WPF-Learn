using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte字节流练习_设置报警温度
{
    public  class SerialPortService
    {

        private SerialPort _serialPort;

        public SerialPortService()
        {
            _serialPort = new SerialPort();

            _serialPort.PortName = "COM1";
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;

            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
                Console.WriteLine("COM1 打开成功");
            }
        }

        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                Console.WriteLine("COM1 已关闭");
            }
        }

        public void Send(byte[] data)
        {
            if (!_serialPort.IsOpen)
            {
                Console.WriteLine("串口没有打开");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("【发送数据】");

            Console.WriteLine(
                $"Byte数量：{data.Length}");

            Console.WriteLine(
                $"HEX：{ProtocolHelper.ByteArrayToHex(data)}");

            _serialPort.Write(data, 0, data.Length);
            Console.WriteLine("发送成功");
        }


        private void SerialPort_DataReceived(object sender,SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(20);

                // 当前串口缓冲区有多少Byte
                int count = _serialPort.BytesToRead;

                if(count <= 0)
                {
                    return;
                }

                // 创建数组
                byte[] buffer = new byte[count];

                // 把串口中的Byte读进数组
                int actualRead = _serialPort.Read(buffer, 0, count);

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("【收到设备回复】");

                Console.WriteLine(
                    $"Byte数量：{actualRead}");

                Console.WriteLine(
                    $"HEX：{ProtocolHelper.ByteArrayToHex(buffer)}");


                Console.WriteLine();
                Console.WriteLine(
                    "逐Byte查看：");

                for(int i = 0; i < buffer.Length; i++)
                {
                    Console.WriteLine($"buffer[{i}] = {buffer[i]} = 0x{buffer[i]:X2}");
                }

                // 解析设备回复
                if(buffer.Length >= 5)
                {
                    bool success = ProtocolHelper.ParseSetAlarmResponse(buffer);

                    Console.WriteLine();

                    Console.WriteLine($"最终结果：{success}");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(@"接收异常:{ex.Message}");
            }
        }

    }
}
