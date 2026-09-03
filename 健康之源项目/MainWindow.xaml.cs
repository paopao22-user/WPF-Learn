using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;

namespace 健康之源项目
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //AppDomain.CurrentDomain.BaseDirectory（应用程序基目录,它永远、严格指向你的 .exe 可执行文件所在的物理文件夹)
        private readonly string _avatarRootDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpLoad", "Avatar"); //动态拼接路径:绝对路径

        // 允许上传的图片后缀
        private readonly string[] _allowExts =
        {
            ".jpg", ".jpeg", ".png", ".gif", ".bmp"
        };

        //单张图片最大2MB
        private const int MaxFileSize = 2 * 1024 * 1024;

        // 当前头像相对路径
        private string? _avatarRelativePath;

        public MainWindow()
        {
            InitializeComponent();

            CreateAvatarDirIfNotExist(); //程序初始化时自动检查并创建目录
        }

        /// <summary>
        /// 程序初始化时自动检查并创建目录
        /// </summary>
        private void CreateAvatarDirIfNotExist()
        {
            //如果不存在对应的目录路径，就自己创建对应的路径
            if (!Directory.Exists(_avatarRootDir))
            {
                Directory.CreateDirectory(_avatarRootDir);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove(); //让窗口跟随鼠标拖动
        }

        /// <summary>
        /// 折叠/展开左侧菜单栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 折叠/展开左侧菜单栏
            MenuBorder.Visibility = MenuBorder.Visibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 点击右上角退出按钮关闭窗口
            this.Close();
        }

        /// <summary>
        /// 点击左侧菜单栏的“员工管理”按钮，打开员工管理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmployeeMenu_Click(object sender, RoutedEventArgs e)
        {
            //UriKind.Relative（相对统一资源标识符类型）：告诉系统：“这是一个相对路径，请以当前软件的根目录为基准去找这个文件”；
            ContentFrame.Source = new Uri("EmpPage.xaml", UriKind.Relative);  //路由寻址构造: 实例化 new Uri("EmpPage.xaml", UriKind.Relative)
        }

        /// <summary>
        /// 点击左侧菜单栏的“会员管理”按钮，打开会员管理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberMenu_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("MemberPage.xaml", UriKind.Relative);
        }

        /// <summary>
        /// 点击左侧菜单栏的“订单管理”按钮，打开订单管理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderMenu_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("OrderPage.xaml", UriKind.Relative);
        }

        /// <summary>
        /// 点击左侧菜单栏的“房间管理”按钮，打开房间管理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoomMenu_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("RoomPage.xaml", UriKind.Relative);
        }

        /// <summary>
        /// 点击左侧菜单栏的“餐桌管理”按钮，打开餐桌管理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableMenu_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("TablePage.xaml", UriKind.Relative);
        }

        /// <summary>
        /// 头像上传，左侧鼠标点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Avatar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //1. 打开文件选择框
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Title = "选择头像图片",
                Filter = "图片文件 (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|" + "*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                Multiselect = false
            };

            //2.  用户点击取消
            if(openDialog.ShowDialog() != true)
            {
                return;
            }

            //3.获取原始文件路径
            string sourceFilePath = openDialog.FileName;

            //4. 校验文件
            if(!CheckFileValid(sourceFilePath, out string errorMsg))
            {
                MessageBox.Show(errorMsg, "图片校验失败", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            //5.获取原始文件后缀
            string fileExt = System.IO.Path.GetExtension(sourceFilePath);

            //6.GUID生成新文件名
            string newFileName = $"{Guid.NewGuid()}{fileExt}";

            //7.目标绝对路径
            string targetFullPath = System.IO.Path.Combine(_avatarRootDir, newFileName);

            //8.业务保存用相对路径
            string saveRelativePath = System.IO.Path.Combine("Upload", "Avatar", newFileName);

            try
            {
                //9. 真正保存图片
                File.Copy(sourceFilePath, targetFullPath, overwrite:true);

                //10.预览新头像
                LoadAvatarPreview(targetFullPath);

                //11.暂时保存相对路径
                _avatarRelativePath = saveRelativePath;

                MessageBox.Show("头像上传成功", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"头像保存失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// 文件校验
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        private bool CheckFileValid(string filePath, out string errorMsg)
        {
            errorMsg = string.Empty;    // 初始化清空错误信息

            FileInfo fileInfo = new FileInfo(filePath); //给路径创建FileInfo对象

            // 1. 文件是否存在
            if (!fileInfo.Exists)
            {
                errorMsg = "选中的文件不存在";

                return false;
            }

            // 2. 文件大小是否超过2MB
            if (fileInfo.Length > MaxFileSize)
            {
                double sizeMb = fileInfo.Length / 1024.0 / 1024.0;
                //  Math.Round(sizeMb, 2): 保留两位小数，例如 3.456MB 会四舍五入为 3.46MB
                errorMsg = $"图片最大允许2MB，当前文件为 {Math.Round(sizeMb, 2)}MB";

                return false;
            }


            // 3. 后缀是否合法
            string ext = fileInfo.Extension.ToLower(); // 文件后缀转为全小写，如 ".PNG" 变为 ".png"

            // 检查 ext 是否存在于白名单数组 _allowExts 中（如 [".jpg", ".png", ".gif"]）
            //item => item == ext（Lambda 表达式/匿名条件函数）：遍历 _allowExts 数组中的每一个 item，看看有没有任何一个等于当前文件的 ext。
            if (!Array.Exists(_allowExts, item => item == ext))
            {
                errorMsg = "仅支持 jpg/jpeg/png/gif/bmp 格式图片";

                return false;
            }

            // 所有检查都通过
            return true;
        }

        /// <summary>
        /// 图片预览
        /// </summary>
        /// <param name="imgPath"></param>
        private void LoadAvatarPreview(string imgPath)
        {
            //BitmapImage 是 WPF 专门用来加载图片文件的对象。
            BitmapImage bitmap = new BitmapImage(); //创建空对象
            //开始配置
            bitmap.BeginInit();  //我要开始设置这个 BitmapImage 了，先别急着加载

            //图片在哪 UriKind.Absolute 是绝对路径，这里的imagPath是绝对路径
            bitmap.UriSource = new Uri(imgPath, UriKind.Absolute);  //告诉 BitmapImage：你要加载的图片具体在哪里。 把 imgPath 这个完整文件路径，包装成一个 URI 对象。

            //怎么加载      OnLoad:在加载阶段一次性把图片数据读进内存，加载完成后就不再一直占着原文件
            bitmap.CacheOption = BitmapCacheOption.OnLoad;


            bitmap.EndInit(); //配置结束，正式应用:配置都写好了，现在按照这些参数真正加载图片


            imgAvatar.Source = bitmap; //把图片显示到界面上。
        }

    }
}