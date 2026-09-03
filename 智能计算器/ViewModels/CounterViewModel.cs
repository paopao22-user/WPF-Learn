using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace 智能计算器.ViewModels
{
    public class CounterViewModel : INotifyPropertyChanged
    {
        //数据存储与通知（属性拦截器）
        private int _count = 5;
        public int Count
        {
            get => _count;
            set
            {
                if (_count != value)    // 防抖：值没变就不处理，避免性能浪费
                {
                    _count = value;     // 1. 存入新值
                    OnPropertyChanged();    // 2. 核心：向界面广播 "Count 变了！"
                }
            }
        }

        // 声明无参命令
        public RelayCommand AddCommand { get; }
        public RelayCommand SubCommand { get; }
        public RelayCommand ResetCommand { get; }

        public CounterViewModel()
        {
            // ✅ 干净清爽的无参 Lambda 写法！
            //execute（执行委托）：定义了用户点击后到底干什么；
            //canExecute（判定委托）：定义了按钮到底什么时候点亮、什么时候置灰。
            // 1. 加一命令：要做的事是 Count++，门槛是 Count < 10
            AddCommand = new RelayCommand(
                execute: () => Count++,
                canExecute: () => Count < 10
            );

            // 2. 减一命令：要做的事是 Count--，门槛是 Count > 0
            SubCommand = new RelayCommand(
                execute: () => Count--,
                canExecute: () => Count > 0
            );

            // 3. 清零命令：要做的事是 Count = 0，门槛是 Count != 0
            ResetCommand = new RelayCommand(
                execute: () => Count = 0,
                canExecute: () => Count != 0
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
