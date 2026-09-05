using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace 设备通讯参数校验面板.ViewModels
{
    public class ParameterRelayCommand : ICommand
    {
        // 存储一个：有参数、无返回值的方法
        private readonly Action<object?> _execute;

        public ParameterRelayCommand(Action<object?> execute)
        {
            _execute = execute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
