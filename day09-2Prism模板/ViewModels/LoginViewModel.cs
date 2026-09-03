using Prism.Commands;
using Prism模板.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Prism模板.ViewModels
{
    public class LoginViewModel
    {
        public DelegateCommand LoginCommand { get; }

        private Login _login;

        public Login Log
        {
            get => _login;

            set
            {
                if(_login != value)
                {
                    _login = value;
                }
            }
        }

        public LoginViewModel()
        {
            _login = new Login()
            {
                Name = "123"
            };

            LoginCommand = new DelegateCommand(Login);
        }

        private void Login()
        {
            MessageBox.Show($"欢迎登录，当前用户名是：{Log.Name}");
        }
    }
}
