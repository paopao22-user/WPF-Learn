using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace 智能登录校验面版改造.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        //1.属性通知
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //2.SetProperty
        // 参数 1：私有字段的内存指针;   参数 2：外界传进来的新值;   参数 3：自动捕获的属性名字
        //加上 ref：相当于把 _userName 在内存里的真实地址钥匙交给了 SetProperty，方法内部执行 field = value; 时，能够直接真金白银地修改外面的 _userName！
        //目标新值 T value: 就是用户在外部输入的新数据（例如输入框里新敲入的 "admin"）
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            // 第 1 行：【判重防抖】检查新旧值是不是一模一样？
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            // 第 2 行：【内存赋值】利用 ref 直接将新值写入私有字段！
            field = value;

            // 第 3 行：【广播发射】通知 XAML 界面刷新该属性对应的控件！
            OnPropertyChanged(propertyName);

            // 第 4 行：【成功标记】返回 true，告诉外面“数据确实发生了改变”！
            return true;
        }

    }
}
