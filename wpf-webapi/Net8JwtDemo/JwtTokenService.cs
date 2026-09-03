using Microsoft.IdentityModel.Tokens;  //主要用到：SymmetricSecurityKey、SigningCredentials、SecurityAlgorithms  负责密钥和签名算法
using System.IdentityModel.Tokens.Jwt; //主要用到：JwtSecurityToken、JwtSecurityTokenHandler  负责生成JWT和解析 JWT Token 转为字符串
using System.Security.Claims;          //主要用到：Claim、ClaimTypes  负责存储在 JWT Token 里的用户信息
using System.Text;                     //主要用到：Encoding.UTF8.GetBytes 负责把字符串密钥转换为 byte[]
using wpf_webapi.Model;

namespace wpf_webapi.Net8JwtDemo
{
    public class JwtTokenService
    {
        private readonly JwtSettings _jwtSettings;

        // 构造函数，接收 JwtSettings 注入
        public JwtTokenService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        // 生成 JWT Token
        public string GenerateToken(string userId,string userName)
        {
            // 1. 创建用户信息（Claim = 存储在 Token 里的信息）
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName)
            };
            // 2. 创建密钥
            var key = new
SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // 3. 生成 Token
            //创建一个 JWT：它由谁签发、发给谁、里面有什么用户信息、什么时候过期，以及用什么密钥和算法签名
            var token = new JwtSecurityToken(  //JwtSecurityToken: 组装整张 JWT
                issuer: _jwtSettings.Issuer,    //签发者
                audience: _jwtSettings.Audience,//接收者
                claims: claims,                 //存储在 Token 里的用户信息
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiresMinutes), //过期时间
                signingCredentials: credentials     //签名凭证，包含密钥和算法
            );
            // 4. 返回 Token 字符串
            //new JwtSecurityTokenHandler():专门处理 JWT 的工具; WriteToken(token):把 JwtSecurityToken 对象转换为标准 JWT 字符串
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
