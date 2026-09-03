// See https://aka.ms/new-console-template for more information

using Byte字节流练习_温度传感器;
//Program 负责菜单调度，Helper 负责算报文，Service 负责推硬件；

// 声明内部类 Program：控制台应用程序的标准承载类，访问权限为程序集内部可见
internal class Program
{
    // MainEntry 主入口方法：整个应用程序启动的唯一第一执行点，主线程由这里顺次向下执行
    static void Main(string[] args)
    {
        // 1. 打印系统启动标题栏，为操作人员呈现清晰的软件标识
        Console.WriteLine(
            "========== Byte 串口通讯练习 ==========");

        // 打印空行，保持控制台视觉舒适度与段落留白
        Console.WriteLine();

        // 明确当前上位机 C# 端占用的本地物理端口标识
        Console.WriteLine(
            "C# 串口：COM1");

        // 明确联调对端（从站/串口助手）监听的虚拟端口标识
        Console.WriteLine(
            "串口调试助手：COM2");

        // 再次打印空行隔离界面提示语
        Console.WriteLine();

        // 2. 核心实例化：创建串口通信服务对象
        // 在这一步，内部封装的 SerialPort 会完成默认通信参数（9600-N-8-1）的内存初始化
        SerialPortService serialService = new SerialPortService();

        // 3. 开启异常防御网：将所有可能引发底层硬件异常的代码包裹在 try 块中
        try
        {
            // 4. 调用服务层的 Open 方法：向操作系统内核申请 COM1 独占句柄，并注册底层接收事件
            serialService.Open();

            // 5. 开启命令监听死循环：让主控制台程序常驻内存运行，防止 Main 函数执行完直接闪退
            while (true)
            {
                // 打印每次循环操作的分隔线，保持交互界面整洁
                Console.WriteLine();
                Console.WriteLine(
                    "======================================");

                // 打印交互操作引导提示语
                Console.WriteLine(
                    "请输入操作：");

                // 提示用户输入 1 可触发温度读取指令的组包与发射
                Console.WriteLine(
                    "1 - 发送读取温度指令");

                // 提示用户输入 q 可退出当前监控交互循环
                Console.WriteLine(
                    "q - 退出");

                // 6. 阻塞主线程等待键盘输入：
                // Console.ReadLine() 会挂起主线程，直到用户敲击回车键；
                // string? 后缀表明该变量支持 C# 可空引用类型，增强类型安全检查
                string? input = Console.ReadLine();

                // 7. 分支路由一：用户选择执行温度采集业务
                if (input == "1")
                {
                    // 7.1 面向意图组包：委托 ProtocolHelper 静态工具类组装符合设备协议的二进制报文
                    // 彻底解耦了业务意图与具体的十六进制字节拼接细节
                    byte[] request = ProtocolHelper.BuildReadTemperatureRequest();

                    // 7.2 面向服务发射：将组装好的纯字节数组通过通信服务推入物理底层驱动
                    serialService.Send(request);
                }
                // 8. 分支路由二：用户选择退出程序
                else if (input == "q")
                {
                    // 执行 break 语句，立刻打破并跳出当前的 while(true) 无限循环
                    break;
                }
                // 9. 分支路由三：防御非法输入（如用户误敲了其他字母或空格）
                else
                {
                    // 给出友好提示，并自动随下一次循环重新展示操作菜单
                    Console.WriteLine("输入错误");
                }
            } // end while 循环体结束点
        }
        // 10. 捕获并处理所有未处理的通信或业务异常（如端口被其他程序强行抢占或拔掉）
        catch (Exception ex)
        {
            // 在控制台打印具体的错误描述信息，供工程人员定位故障根因
            Console.WriteLine($"程序异常：{ex.Message}");
        }
        // 11. 终极资源释放保障：无论 try 块中是正常 break 退出，还是突发异常崩溃闪退，
        // finally 块里的代码都保证 100% 必定被 CLR 执行！
        finally
        {
            // 确保关闭 COM1 端口，将操作系统串口句柄优雅归还给系统，避免下次启动报“访问被拒绝”
            serialService.Close();
        }

        // 12. 程序退出提示：标志着整个软件生命周期安全、圆满地结束
        Console.WriteLine("程序结束");
    } // end Main 方法结束点
}