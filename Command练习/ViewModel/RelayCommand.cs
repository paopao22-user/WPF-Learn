using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Command练习.ViewModel
{
    public class RelayCommand :ICommand
    {
        private readonly Action _execute;

        private readonly Func<bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;
        

        public RelayCommand(Action execute, Func<bool>? canExeCute = null)
        {
            _execute = execute;

            _canExecute = canExeCute;
        }

        public void Execute(object? parameter)
        {
            _execute();
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
