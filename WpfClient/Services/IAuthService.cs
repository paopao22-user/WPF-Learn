using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Model;

namespace WpfClient.Services
{
    public interface IAuthService
    {
        // 传入登录 DTO，返回 Token 字符串（或 null）
        Task<string?> LoginAsync(Logindto dto);
    }
}
