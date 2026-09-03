using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wpf_webapi.DbContext;

namespace wpf_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController:ControllerBase
    {
        private readonly UserDbContext _context;
        // 通过构造函数把刚才注册的 UserDbContext 注入进来
        public PersonController(UserDbContext context)
        {
            _context = context;
        }
        // GET: api/Person
        [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            var list = await _context.Persons.ToListAsync();
            return Ok(list);
        }
    }
}
