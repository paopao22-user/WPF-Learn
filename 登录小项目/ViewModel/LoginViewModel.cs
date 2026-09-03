using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using 登录小项目.Model;

namespace 登录小项目.ViewModel
{
    public class LoginViewModel: INotifyPropertyChanged
    {
        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        //【你在代码编辑器中写的源码】
        // public string Age
        //        {
        //            set { _age = value; OnPropertyChanged(); } // 括号内完全留空
        //        }
        //                    │
        //                    ▼ (点击生成 / 编译为 IL 中间语言)
        //【C# 编译器侦测到底层带有 [CallerMemberName] 特性】
        //                    │
        //                    ▼ (编译器自动提取当前所在属性名 "Age"，并作为默认实参塞入)
        //【编译器在后台偷偷生成的真实底层代码】
        // public string Age
        //        {
        //            set { _age = value; OnPropertyChanged("Age"); } // 自动无差错填入 "Age"！
        //        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            if (UserName == "admin" && Password == "123")
            {
                Message = "登录成功";
            }
            else
            {
                Message = "登录失败";
            }
        }

        // 3. 属性改变事件通知机制的工作原理示意图：
        //[发起端 Caller: 某个业务代码修改属性] ──► user.EmployeeName = "李四"
        //                                        │
        //                                        ▼ (进入属性的 set 访问器)
        //[触发当前方法] ──► OnPropertyChanged(); // 自动捕获 propertyName = "EmployeeName"
        //                        │
        //                        ▼
        //[执行广播发射] ──► PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EmployeeName"))
        //                        │
        //                        ▼ (广播沿绑定管道传递)
        //[接收端 Receiver: WPF 绑定引擎(Binding Engine)]
        //                        │
        //                        ▼ (匹配到 XAML 中的<TextBlock Text="{Binding EmployeeName}"/>)
        //[最终输出 Final Output: TextBlock 重新拉取 "李四"，屏幕上的文字瞬间更新！]

        // 1. 属性改变事件通知接口
        public event PropertyChangedEventHandler? PropertyChanged; //PropertyChangedEventHandler：微软预定义好的专用委托类型，规定了广播消息必须
                                                                   //包含 (object sender, PropertyChangedEventArgs e) 两个参数。

        // 2. 升级后的通知方法：受保护的 + 自动捕获调用方名字 + 默认值 null
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //PropertyChanged?（空条件运算符安全检查）：
        //如果目前没有任何 UI 控件绑定它（PropertyChanged == null），什么都不做，安全跳过，绝不报空指针异常；
        //.Invoke(...)（唤醒执行方法）：
        //如果有控件在收听（PropertyChanged != null），立刻扣动扳机，把消息发射给所有收听此广播的 UI 控件！
        //new PropertyChangedEventArgs(propertyName)：把属性名字（如 "EmployeeName"）打包进事件信封里。
    }
}
