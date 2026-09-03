using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Byte字节流练习_温度传感器
{
    public class SerialPortService
    {
        private SerialPort _serialPort; // 串口对象，用于与硬件设备进行通信

        public SerialPortService()
        {
            _serialPort = new SerialPort();

            // C# 使用 COM1
            _serialPort.PortName = "COM1";

            // 波特率
            _serialPort.BaudRate = 9600;

            // 数据位
            _serialPort.DataBits = 8;

            // 停止位
            _serialPort.StopBits = StopBits.One;

            // 校验位
            _serialPort.Parity = Parity.None;

            // 注册串口接收事件
            _serialPort.DataReceived += SerialPort_DataReceived;
        }


        /// <summary>
        /// 打开串口
        /// </summary>
        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open(); // 打开串口

                Console.WriteLine(
                    $"串口 {_serialPort.PortName} 已打开");

                Console.WriteLine(
                    "参数：9600 / 8 / N / 1");
            }
        }


        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close(); // 关闭串口

                Console.WriteLine("串口已关闭");
            }
        }


        /// <summary>
        /// 串口底层发射执行器：将内存中的原始二进制字节数组推入硬件物理线路
        /// </summary>
        /// <param name="data">要向外发射的完整字节数组 (包含报文帧头、指令、数据及校验码)</param>
        public void Send(byte[] data)
        {
            // 1. 入参非空安全防御：防止上层业务误传 null 导致后续引发 NullReferenceException 崩溃
            if (data == null || data.Length == 0)
            {
                Console.WriteLine("发送数据为空，取消发送");
                return;
            }

            // 2. 状态门禁卫语句：检查串口是否正处于打开可用状态
            // 如果串口未连接（如断线、被抢占），坚决不执行写入，避免引发底层抛异常闪退
            if (!_serialPort.IsOpen)
            {
                Console.WriteLine("串口没有打开");
                return;
            }

            // 3. 打印准备发射的分隔线提示
            Console.WriteLine();
            Console.WriteLine("【C# 准备发送】");

            // 4. 打印本次发射的载荷长度：方便排查组包长度是否符合设备协议规范
            Console.WriteLine($"Byte 数量：{data.Length}");

            // 5. 发送前存证留痕：调用 ByteArrayToHex 将即将发射的字节以 Hex 打印，留下不可抵赖的出厂日志
            Console.WriteLine($"HEX：{ProtocolHelper.ByteArrayToHex(data)}");

            // 6. 核心物理写入：调用 Windows 底层驱动 WriteFile API 将字节推入硬件发送缓冲区
            // 参数说明：
            //   data：存放要发送字节的数据源数组
            //   0：从 data 数组的第 0 个下标（起点）开始传输
            //   data.Length：本次连续向外写入的字节总数
            _serialPort.Write(data, 0, data.Length);

            // 7. 发送状态反馈：告知控制台数据已成功脱离 C# 托管堆、顺利装填进操作系统的发送管道
            Console.WriteLine("发送完成");
        }


        /// <summary>
        /// 串口接收事件回调函数：只要 Windows 驱动检测到有数据到达，底层线程池就会自动调用此方法
        /// </summary>
        /// <param name="sender">触发事件的源对象 (即 _serialPort 实例)</param>
        /// <param name="e">串口事件参数对象</param>
        private void SerialPort_DataReceived(object sender,SerialDataReceivedEventArgs e)
        {
            try
            {
                // 1. 硬件通信延时缓冲：
                // 串口数据是逐个比特串行爬进来的，事件触发时整帧可能还没收齐；
                // 延时 20 毫秒，给后续字节留出进入 Windows 驱动缓冲区的物理时间，防止半包。
                Thread.Sleep(20);

                // 2. 探查当前缓冲区字节总量：
                // 向操作系统底层驱动询问：当前输入缓冲区中一共积攒了多少个可读字节
                int bytesToRead = _serialPort.BytesToRead;

                // 3. 安全防御拦截：如果缓冲区为空（0 字节），直接退出，避免执行无意义的代码
                if (bytesToRead <= 0) return;

                // 4. 打印调试分界线与标题：方便开发者在控制台清晰区分每一次数据到达动作
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("【DataReceived 事件触发】");

                // 5. 打印诊断数量：记录当前这一批次接收到了多少个字节
                Console.WriteLine($"缓冲区 Byte 数量：{bytesToRead}");

                // 6. 动态按需分配内存：
                // 缓冲区有几个字节，就精准实例化多大尺寸的 byte 数组，不浪费一分内存，杜绝数组越界
                byte[] buffer = new byte[bytesToRead];  // 预分配一个与缓冲区字节数相同大小的数组，准备搬运数据

                // 7. 执行物理读取：
                // 从串口缓冲区将字节流一次性搬运到 buffer 数组中
                // 参数说明：buffer(目标内存桶), 0(从桶的第0位开始装), bytesToRead(计划搬运的字节总数)
                // 返回值 actualRead：记录操作系统实际成功读出的字节数量
                int actualRead = _serialPort.Read(buffer, 0, bytesToRead);

                // 8. 打印实际搬运出来的字节数，验证是否与预期一致
                Console.WriteLine($"实际读取 Byte 数量：{actualRead}");

                // 9. 调用辅助方法转换格式：将冰冷的二进制字节数组，转换为带空格的人眼可读 Hex 字符串并打印
                Console.WriteLine($"收到 HEX：{ProtocolHelper.ByteArrayToHex(buffer)}");

                // 10. 打印逐字节分析引导文本
                Console.WriteLine();
                Console.WriteLine("逐 Byte 查看：");

                // 11. 循环遍历数组中的每一个字节，做微观结构对照排查
                for (int i = 0; i < buffer.Length; i++)
                {
                    // buffer[i],3：表示十进制显示并强制右对齐占 3 个字符宽度，保证排版对齐
                    // buffer[i]:X2：表示输出 2 位大写的十六进制，不足两位自动在前面补 0
                    Console.WriteLine($"buffer[{i}] = 十进制 {buffer[i],3} | Hex 0x{buffer[i]:X2}");
                }

                // 12. 协议合法性前置长度检查：
                // 我们的工业温度报文格式为 [帧头 站号 功能码 长度 高字节 低字节]，固定至少 6 字节；
                // 只有达到或超过 6 字节时，才有资格进入业务解包流程，避免截断数据引发异常
                if (buffer.Length >= 6)
                {
                    // 13. 移交协议解析器：调用解包算法，提取温度字节并换算为实际摄氏度浮点数
                    double temperature = ProtocolHelper.ParseTemperature(buffer);

                    // 14. 业务落地输出：在控制台打印最终转换出的实际工程量温度
                    Console.WriteLine();
                    Console.WriteLine($"最终解析结果：{temperature}℃");
                }
            }
            catch (Exception ex)
            {
                // 15. 防御性异常捕获：捕获超时、端口拔出等物理突发故障，保证上位机主程序不崩溃
                Console.WriteLine($"接收数据异常：{ex.Message}");
            }
        }
    }
}
