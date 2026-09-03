// See https://aka.ms/new-console-template for more information

using Byte字节流练习_压力传感器;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(
            "========== 压力传感器串口练习 ==========");

        Console.WriteLine();

        Console.WriteLine("C#：COM1");

        Console.WriteLine("串口助手：COM2");

        // 创建串口服务对象
        SerialPortService serialService = new SerialPortService();


        try
        {
            serialService.Open();   // 打开串口


            while (true)
            {
                Console.WriteLine();

                Console.WriteLine("1 - 读取压力");

                Console.WriteLine("q - 退出");

                // 读取用户输入,用ReadLine()来卡住界面
                string? input = Console.ReadLine();


                if (input == "1")
                {
                    // ① 组包
                    byte[] request = ProtocolHelper.BuildReadPressureRequest();


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
            // 关闭串口，释放资源
            serialService.Close();
        }
    }
}
