using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace 智能登录校验面板.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {

        //01.属性
        private  string _username = string.Empty;

        public string UserName
        {
            get => _username;

            set
            {
                if(_username != value)          //1.手动判重
                {
                    _username = value;          // 2. 手动赋值

                    OnPropertyChanged();        // 3. 手动广播

                    LoginCommand.RaiseCanExecuteChange();
                    ResetCommand.RaiseCanExecuteChange();
                }
            }
        }


        private string _password = string.Empty;

        public string Password
        {
            get => _password;

            set
            {
                if(_password != value)
                {
                    _password = value;
                    OnPropertyChanged();

                    LoginCommand.RaiseCanExecuteChange();
                    ResetCommand.RaiseCanExecuteChange();
                }
            }
        }


        // 02.命令
        //声明命令
        public RelayCommand LoginCommand { get; }
        public RelayCommand ResetCommand { get; }

        //构造函数
        public LoginViewModel()
        {
            // 1. 登录命令：挂接登录业务与校验门槛
            LoginCommand = new RelayCommand(execute: DoLogin, canexecute: CanLogin);

            // 2. 清空命令：任一框有字时才允许清空
            ResetCommand = new RelayCommand(execute: () => { UserName = ""; Password = ""; },
                                            canexecute: () => !string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(Password));
        }

        private void DoLogin()
        {
            MessageBox.Show($"🎉 恭喜！用户 [{UserName}] 登录成功！", "登录提示");
        }

        // 门槛规则：账号非空 且 密码 ≥ 6 位
        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(UserName) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   Password.Length >= 6;
        }


        //03.属性通知
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
