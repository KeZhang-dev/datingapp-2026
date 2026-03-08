using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")] // localhost:5000/api/members
    [ApiController]
    public class MembersController(AppDbContext context) : ControllerBase
    {
        //这是一个简单的API控制器，包含两个GET方法，一个获取所有成员。
        // 然后加了一个async，因为用了async/awit就可以异步执行数据库查询，提升性能。
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var menbers = await context.Users.ToListAsync();
            return menbers;
        }

        //这一个GET方法根据ID获取特定成员。
        // 然后加了一个async，因为用了async/awit就可以异步执行数据库查询，提升性能。
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            var member = await context.Users.FindAsync(id);
            if (member == null) return NotFound();
            return member;
        }
    }
}
