using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeviceMonitorCenter.ViewModels
{
    public class RelayCommandOfT<T> : ICommand
    {
        private readonly Action<T>? _execute;

        

        public event EventHandler? CanExecuteChanged;

        public RelayCommandOfT(Action<T>? execute)
        {
            _execute = execute;
            
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute((T)parameter!);
        }


        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
