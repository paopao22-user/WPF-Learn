using System.Net.Http;
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
using WpfClient.Common;
using WpfClient.Model;
using WpfClient.Services;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 1. 声明 HttpClient 和 AuthService
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IAuthService _authService;
        public MainWindow()
        {
            InitializeComponent();
            // 2. 配置 WebAPI 实际监听的端口地址（注意末尾必须有 /）
            _httpClient.BaseAddress = new Uri("http://localhost:5083/");
            // 3. 将配好的 _httpClient 注入到 AuthService 中
            _authService = new AuthService(_httpClient);
        }

        // 4. 按钮点击事件处理程序 (Event Handler)
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // 组装登录入参（请确保账号密码与你的 WebAPI 逻辑或数据库一致）
            var dto = new Logindto
            {
                UserName = "admin",
                Password = "123456" // 如果你 WebAPI 写的是 123456，这里就填 123456
            };
            // 发起异步登录请求
            string? token = await _authService.LoginAsync(dto);


            if (!string.IsNullOrEmpty(token))
            {
                // ==========================================
                //  核心步骤：将 Token 保存到全局 AppSession 中！
                // ==========================================
                AppSession.AccessToken = token;
                AppSession.CurrentUserName = dto.UserName;
                MessageBox.Show($"登录成功并已存入会话！\n欢迎您：{AppSession.CurrentUserName}\n\nToken：\n{AppSession.AccessToken}");
            }
            else
            {
                // 失败：提示错误
                MessageBox.Show("登录失败，请检查账号密码或 WebAPI 服务状态！");
            }
        }
    }
}