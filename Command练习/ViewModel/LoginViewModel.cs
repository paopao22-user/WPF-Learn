using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Command练习.ViewModel
{
    public class LoginViewModel:INotifyPropertyChanged
    {
        public RelayCommand LoginCommand { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login, CanLogin); //
        }

        /// <summary>
        /// 登录逻辑
        /// </summary>
        private void Login()
        {
            MessageBox.Show($"用户名：{UserName}\n密码：{PassWord}");
        }

        /// <summary>
        /// 能否登录，判断
        /// </summary>
        /// <returns></returns>
        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(PassWord);
        }

        private string _userName = "";

        public string UserName
        {
            get => _userName;

            set
            {
                if(_userName != value)
                {
                    _userName = value;

                    OnPropertyChanged();

                    LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _passWord = "";

        public string PassWord
        {
            get => _passWord;

            set
            {
                if(_passWord != value)
                {
                    _passWord = value;

                    OnPropertyChanged();

                    LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;  

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
