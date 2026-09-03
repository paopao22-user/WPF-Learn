using Microsoft.AspNetCore.Mvc;
using wpf_webapi.Net8JwtDemo;

namespace wpf_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController:ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;

        public LoginController(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]  //这个 Login 方法处理 POST 请求
        public IActionResult Login(string userName, string pwd)
        {
            // 真实项目：这里校验数据库账号密码
            if(userName == "admin" && pwd == "123456")
            {
                // 生成 Token
                var token = _jwtTokenService.GenerateToken("1", userName);
                return Ok(new { Token = token });
            }
            return Unauthorized("账号或密码错误");

        }
    }
}
