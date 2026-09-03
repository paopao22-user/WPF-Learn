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

namespace 健康之源项目
{
    /// <summary>
    /// EmpEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EmpEditWindow : Window
    {
        /// <summary>
        /// 获取或设置员工姓名
        /// </summary>
        public string EmployeeName
        {
            get => NameTextBox.Text;
            set => NameTextBox.Text = value;
        }

        /// <summary>
        /// 获取或设置员工年龄
        /// </summary>
        public int EmployeeAge
        {
            get
            {
                int.TryParse(AgeTextBox.Text, out int age);
                return age;
            }

            set
            {
                AgeTextBox.Text = value.ToString();
            }
        }

        /// <summary>
        /// 获取或设置员工生日
        /// </summary>
        public DateTime EmployeeBirthday
        {
            get
            {
                return BirthdayPicker.SelectedDate
                       ?? DateTime.Now;
            }

            set
            {
                BirthdayPicker.SelectedDate =
                    value;
            }
        }

        /// <summary>
        /// 获取或设置员工地址
        /// </summary>
        public string EmployeeAddress
        {
            get => AddressTextBox.Text;

            set => AddressTextBox.Text = value;

        }

        public EmpEditWindow()
        {
            InitializeComponent();
            BirthdayPicker.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// 确认按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            //1.姓名校验
            if (string.IsNullOrWhiteSpace(EmployeeName))
            {
                MessageBox.Show("请输入姓名");
                return;
            }

            //2.年龄校验
            if(!int.TryParse(AgeTextBox.Text, out int age))
            {
                MessageBox.Show("年龄必须是数字");
                return;
            }
            if(age <= 0 || age > 120)
            {
                MessageBox.Show("请输入正确的年龄");
                return;
            }

            //3.生日校验
            if(BirthdayPicker.SelectedDate == null)
            {
                MessageBox.Show("请选择日期");
                return;
            }
            if(BirthdayPicker.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("生日不能大于当天");
                return;
            }

            //4.地址校验
            if (string.IsNullOrWhiteSpace(EmployeeAddress))
            {
                MessageBox.Show("请输入地址");
                return;
            }

            //5.所有校验都通过
            DialogResult = true;
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
