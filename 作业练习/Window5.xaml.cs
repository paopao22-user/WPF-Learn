using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace 作业练习
{
    /// <summary>
    /// Window5.xaml 的交互逻辑
    /// </summary>
    public partial class Window5 : Window
    {
        public Window5()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //1.判断开始日期和结束日期是否未选择
            if(dpStart.SelectedDate == null || dpEnd.SelectedDate == null)
            {
                MessageBox.Show("请选择开始日期或者结束日期");
                return;
            }

            //2.获取开始日期和结束日期
            DateTime dtStart = dpStart.SelectedDate.Value;

            DateTime dtEnd = dpEnd.SelectedDate.Value;



            //3.打印开始日期和结束日期
            MessageBox.Show($"开始日期为{dtStart: yyyy-MM-dd},结束日期为{dtEnd: yyyy-MM-dd}");
        }

        /// <summary>
        /// 选中日期改变时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDate.SelectedDate.HasValue)
            {
                DateTime dtDate = dpDate.SelectedDate.Value;
                MessageBox.Show($"展示日期为{dtDate: yyyy-MM-dd}");
            }
        }
    }
}
