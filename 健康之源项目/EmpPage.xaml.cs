using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 健康之源项目
{
    /// <summary>
    /// EmpPage.xaml 的交互逻辑
    /// </summary>
    public partial class EmpPage : Page
    {
        // 当前员工数据表
        private DataTable _empTable;
        public EmpPage()
        {
            InitializeComponent();
            // 创建测试数据
            _empTable = GetData();
            EmpDate.ItemsSource = _empTable.DefaultView; // 绑定数据源
        }


        private DataTable GetData()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("Age");
            dt.Columns.Add("Bir");
            dt.Columns.Add("Address");

            for (int i = 1; i <= 10; i++)
            {
                DataRow dataRow = dt.NewRow();

                dataRow[0] = i;
                dataRow[1] = "张三" + i;
                dataRow[2] = 18 + i;
                dataRow[3] = DateTime.Now;
                dataRow[4] = "湖南 长沙";

                dt.Rows.Add(dataRow);
            }

            return dt;
        }

        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //1.准备弹窗
            //1.1创建新窗口
            EmpEditWindow empEdit = new EmpEditWindow();
            //1.2 把主窗体设为父级，保证弹窗居中、不被遮挡
            //empEdit.Owner   ──► 谁是我的宿主爹？(确立从属关系) ;Window.GetWindow(this)  ──► 沿着当前 Page 向上爬树找大窗口
            empEdit.Owner = Window.GetWindow(this);//Window.GetWindow(...):它会沿着 WPF 的 VisualTree（视觉树） 一层层向上遍历父级容器，直到找到最顶层的那个 Window 为止！

            //2.模态交互
            //2.1弹出窗口并阻断代码，等待用户在弹窗输入并点击“确定”
            bool? result = empEdit.ShowDialog();  //模态打开子窗体，返回结果

            //3.数据落地
            //3.1 只有用户点了“确定”，才开始落表
            if(result == true)
            {
                //3.2创建一条空白的行数据，准备添加到datagrid里面
                DataRow row = _empTable.NewRow();  //创建一个新的行数据

                //3.3 装填数据：编号自增，其他字段从弹窗属性提取
                row["Id"] = _empTable.Rows.Count + 1; //行的id
                row["Name"] = empEdit.EmployeeName; //新窗体的员工姓名
                row["Age"] = empEdit.EmployeeAge;
                row["Bir"] = empEdit.EmployeeBirthday;
                row["Address"] = empEdit.EmployeeAddress;

                //3.4 正式加入总表集合
                _empTable.Rows.Add(row);
            }

        }

        /// <summary>
        /// 编辑图片，鼠标左键点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;

            DataRowView rowView = image?.DataContext as DataRowView;

            if (rowView == null)
                return;

            EmpEditWindow empEdit02 = new EmpEditWindow();  //创建子窗体

            //旧数据回显
            empEdit02.EmployeeName = rowView["Name"].ToString() ?? "";
            empEdit02.EmployeeAge = Convert.ToInt32(rowView["Age"]);
            empEdit02.EmployeeBirthday = Convert.ToDateTime(rowView["Bir"]);
            empEdit02.EmployeeAddress = rowView["Address"].ToString() ?? "";

            empEdit02.Owner = Window.GetWindow(this);  // 把主窗体设为父类窗体

            bool? result = empEdit02.ShowDialog();

            //新数据写回当前行
            if(result == true)
            {
                rowView["Name"] = empEdit02.EmployeeName;
                rowView["Age"] = empEdit02.EmployeeAge;
                rowView["Bir"] = empEdit02.EmployeeBirthday;
                rowView["Address"] = empEdit02.EmployeeAddress;
            }
        }

        /// <summary>
        /// 删除图片，鼠标左键点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image? image = sender as Image; // sender 就是被点击的 Image, 可以强行转换
            //DataGrid 表格在绘制每一行时，会给整行绑定一个员工数据包DataContext；放在这一行里面的所有子控件（包括 StackPanel 和 Image），天然会自动继承祖先分配给这一行的数据包
            //当我们执行 EmpDate.ItemsSource = _empTable.DefaultView; 时，表格绑定的其实是 DataView（数据视图）；
            //在 DataView 中，每一条记录的包装盒就叫做 DataRowView（数据行视图包装对象）；
            //把它理解为一个“塑料保护壳”，真正的核心数据 DataRow 就装在这个保护壳的.Row 属性里面。
            DataRowView? rowView = image?.DataContext as DataRowView; //把这张图片的数据上下文 强转为 DataRowView

            if (rowView == null)
                return;

            MessageBoxResult result = MessageBox.Show($"确定删除员工 {rowView["Name"]} 吗？", "删除确认",
           MessageBoxButton.YesNo, MessageBoxImage.Question);  //弹出是否删除的窗口，进行选择

            //如果选择删除，就从数据表_empTable 里面的行数据集合删除这一行
            if (result == MessageBoxResult.Yes)
            {
                _empTable.Rows.Remove(rowView.Row); //rowView.Row 代表这一行的数据
            }
        }
    }
}
