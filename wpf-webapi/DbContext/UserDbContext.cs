using Microsoft.EntityFrameworkCore;
using wpf_webapi.Model;

namespace wpf_webapi.DbContext
{
    public class UserDbContext: Microsoft.EntityFrameworkCore.DbContext
    {
        // 映射到数据库的两张表
        public DbSet<Person> Persons { get; set; }
        public DbSet<Province> Provinces { get; set; }

        // 构造函数：接收外部传入的数据库配置参数
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
    }
}
