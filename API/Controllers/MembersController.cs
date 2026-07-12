using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;

namespace API.Controllers
{
    [Authorize] // 只有经过身份验证的用户才能访问这个控制器中的方法
    public class MembersController(IMemberRepository memberRepository) : BaseApiController
    {
        // GET方法获取所有成员。
        // 这个方法是异步的，因为它使用了async/await来异步执行数据库查询，从而提高性能。
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        {
            return Ok(await memberRepository.GetMembersAsync());
        }

        // GET方法获取1个成员的详细信息，使用成员的ID作为参数。
        // 如果找不到该成员，则返回404 Not Found。
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(string id)
        {
            var member = await memberRepository.GetMemberByIdAsync(id);
            if (member == null) return NotFound();
            return member;
        }

        // GET方法获取1个成员的所有照片，使用成员的ID作为参数。
        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            return Ok(await memberRepository.GetPhotosByMemberIdAsync(id));
        }

        // PUT方法更新成员信息，使用MemberUpdateDto作为参数。
        // 这个方法首先从JWT中获取当前用户的ID，然后使用该ID从数据库中获取成员信息。
        // 如果找不到该成员，则返回400 Bad Request。
        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var memberId = User.GetMemberId();

            var member = await memberRepository.GetMemberForUpdate(memberId);
            if (member == null) return BadRequest("Could not find member");

            member.DisplayName = memberUpdateDto.DisplayName ?? member.DisplayName;
            member.Description = memberUpdateDto.Description ?? member.Description;
            member.City = memberUpdateDto.City ?? member.City;
            member.Country = memberUpdateDto.Country ?? member.Country;

            member.User.DisplayName = memberUpdateDto.DisplayName ?? member.User.DisplayName;

            if (await memberRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update member");
        }

    }
}
