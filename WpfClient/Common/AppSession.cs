using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Common
{
    /// <summary>
    /// 全局用户会话管理类（静态单例）
    /// </summary>
    public static class AppSession
    {
        /// <summary>
        /// 当前登录成功用户的 JWT 访问令牌 (Token)
        /// </summary>
        public static string? AccessToken { get; set; }
        /// <summary>
        /// 当前登录的用户名（可选，方便在界面顶部显示“欢迎你，admin”）
        /// </summary>
        public static string? CurrentUserName { get; set; }
        /// <summary>
        /// 是否处于已登录状态（只读计算属性）
        /// </summary>
        public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(AccessToken);
        /// <summary>
        /// 退出登录 / 清空会话
        /// </summary>
        public static void Clear()
        {
            AccessToken = null;
            CurrentUserName = null;
        }
    }
}
