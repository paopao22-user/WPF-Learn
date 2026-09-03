using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Model;

namespace WpfClient.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        // 构造函数传入 HttpClient
        public AuthService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            
        }

        public async Task<string?> LoginAsync(Logindto dto)
        {
            // 拼接请求 URL
            string url = $"api/Login?userName={dto.UserName}&pwd={dto.Password}";

            // 发起 POST 请求
            HttpResponseMessage response = await _httpClient.PostAsync(url, null);// 这里传入 null，因为我们已经把参数放在 URL 中了
            if (response.IsSuccessStatusCode)
            {
                // 使用刚刚写好的 LoginResponse DTO 进行解析
                //ReadFromJsonAsync<LoginResponse>()（JSON 异步反序列化拓展方法）： 属于 System.Net.Http.Json 命名空间。它会自动读取网络流中的 JSON 文本，
                //并利用反射将 JSON 中的属性名（"token"）与你的 LoginResponse 类中的属性名（Token）进行匹配赋值，最终在内存中生成一个 LoginResponse 的实例对象！
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>(); // 解析响应内容为 LoginResponse 对象
                return result?.Token; // 返回 Token
            }
            return null; // 登录失败返回 null
        }
    }
}
