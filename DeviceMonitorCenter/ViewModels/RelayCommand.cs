using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeviceMonitorCenter.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action? _execute;

        private readonly Func<bool>? _canexecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action? execute, Func<bool>? canexecute)
        {
            _execute = execute;
            _canexecute = canexecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canexecute == null || _canexecute();
        }

        public void Execute(object? parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
