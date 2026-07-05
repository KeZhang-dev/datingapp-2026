using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize] // 只有经过身份验证的用户才能访问这个控制器中的方法
    public class MembersController(IMemberRepository memberRepository) : BaseApiController
    {
        //这是一个简单的API控制器，包含两个GET方法，一个获取所有成员。
        // 然后加了一个async，因为用了async/awit就可以异步执行数据库查询，提升性能。
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        {
            return Ok(await memberRepository.GetMembersAsync());
        }

        //这一个GET方法根据ID获取特定成员。
        // 然后加了一个async，因为用了async/awit就可以异步执行数据库查询，提升性能。
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(string id)
        {
            var member = await memberRepository.GetMemberByIdAsync(id);
            if (member == null) return NotFound();
            return member;
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            return Ok(await memberRepository.GetPhotosByMemberIdAsync(id));
        }
    }
}
