using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace 设备通讯参数校验面板.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        //1. 属性通知
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        //2. SetProperty
        protected bool SetProperty<T>(ref T field, T value,[CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            field = value;

            OnPropertyChanged(propertyName);

            return true;
        }
    }
}
