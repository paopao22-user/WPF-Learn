// See https://aka.ms/new-console-template for more information

using Byte字节流练习_设置报警温度;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine( "========== 设置温度报警值 ==========");

        Console.WriteLine();

        Console.WriteLine("C#：COM1");

        Console.WriteLine("设备模拟器：COM2");


        SerialPortService serialService = new SerialPortService();  // 创建串口服务对象


        try
        {
            serialService.Open(); // 打开串口


            while (true)
            {
                Console.WriteLine();

                Console.WriteLine("1 - 设置报警温度");

                Console.WriteLine("q - 退出");


                string? input = Console.ReadLine();


                if (input == "1")
                {
                    Console.WriteLine("请输入报警温度，例如 45.6：");


                    string? temperatureText = Console.ReadLine();   // 读取用户输入的温度值


                    if (!double.TryParse(temperatureText,out double temperature))
                    {
                        Console.WriteLine("温度格式错误");

                        continue;
                    }


                    // ① 组包
                    byte[] request = ProtocolHelper.BuildSetAlarmRequest(temperature);


                    // ② 发送
                    serialService.Send(request);
                }

                else if (input == "q")
                {
                    break;
                }

                else
                {
                    Console.WriteLine("输入错误");
                }
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine($"程序异常：{ex.Message}");
        }

        finally
        {
            serialService.Close();  // 关闭串口
        }
    }
}