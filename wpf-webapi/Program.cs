

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using wpf_webapi.Model; // 引入包含 JwtSettings 的命名空间
using wpf_webapi.DbContext;
using wpf_webapi.Net8JwtDemo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//// 注册 DbContext 服务，并指定使用 SQL Server
builder.Services.AddDbContext<UserDbContext>(options =>
{
    // "DefaultConnection" 要和你 appsettings.json 里 ConnectionStrings 的名字保持一致
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// ===================== 1. 读取 JWT 配置并注入 =====================
//从配置文件找到 `JwtSettings` 节点，并转换成一个 `JwtSettings` C# 对象。
var JwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
// 将配置以单例模式注入容器，方便后面生成 Token 的服务使用
//! 可理解为：告诉 C# 编译器：“我认为这里不会是 null。
builder.Services.AddSingleton(JwtSettings!);


// ===================== 2. 注册 JWT 认证 =====================
//builder.Services.AddAuthentication() : 可以理解为 给 ASP.NET Core 注册 Authentication 身份认证功能。
builder.Services.AddAuthentication(options =>
{
    //Authenticate = 检查你是谁;   Challenge = 你没通过认证时怎么处理
    // 默认使用 JWT 认证
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //当 ASP.NET Core 需要确认“这个用户是谁”时，默认使用 JWT Bearer 认证。
    // Scheme可以理解为: 认证方案 / 认证方式的名字。 ASP.NET Core 可能支持很多认证,这里使用JWT Bearer认证
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // 如果身份认证失败，默认也按照 JWT Bearer 的方式处理认证失败。
})
    //.AddJwtBearer(options =>{...}); 这一步才真正告诉框架：Bearer 里面装的是 JWT，请按照 JWT 规则验证。
    .AddJwtBearer(options =>    
    {
        //`TokenValidationParameters` 可以直接理解成JWT 检查清单，服务器按照这张表检查 Issuer、Audience、有效期和签名。文档配置的正是这四类核心验证。
        options.TokenValidationParameters = new TokenValidationParameters
    {
        // 验证签发者
        ValidateIssuer = true,      //要不要检查签发者？ 返回bool类型 true代表检查
            ValidIssuer = JwtSettings!.Issuer,  //正确的签发者是谁？   返回string类型，即寄件人，这张 Token 是谁签发的
        // 验证接收者
            ValidateAudience = true,    //要不要检查接受者?  返回bool类型  true代表检查接受者
        ValidAudience = JwtSettings.Audience,   //返回string类型，即 接信人。 这张Token是发给谁的
        // 验证过期时间
        ValidateLifetime = true,    //检查 JWT 有没有过期。

            // 验证密钥
            ValidateIssuerSigningKey = true,    //验证 Token 的签名是否合法。
            IssuerSigningKey = new  //分三步： 01.JwtSettings.SecretKey 拿字符串密钥 02.Encoding.UTF8.GetBytes 转换string类型 -> byte[]，密码学底层是二进制，需要byte[]
                                    //03.new SymmetricSecurityKey(...)生成JWT 使用的对称密钥对象
SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey))
    };
    });

builder.Services.AddScoped<JwtTokenService>();  //把 JwtTokenService 注册到 DI 容器

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
// ===================== 3. 必须启用 认证 & 授权 中间件 =====================
app.UseAuthentication(); // 认证（校验Token）  让进入 Web API 的 HTTP 请求真正执行身份认证
app.UseAuthorization();  // 授权（校验权限）

app.MapControllers(); // 映射控制器路由，告诉 ASP.NET Core：你要使用 Controller 里的路由规则来处理 HTTP 请求。

app.Run();
