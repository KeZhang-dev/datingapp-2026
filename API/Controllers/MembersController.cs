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
    public class MembersController(IMemberRepository memberRepository, IPhotoService photoService) : BaseApiController
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

        [HttpPost("add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto([FromForm] IFormFile file)
        {
            var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());
            if (member == null) return BadRequest("Cannot update member");

            var result = await photoService.UploadPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                MemberId = User.GetMemberId()
            };

            if (member.ImageUrl == null)
            {
                member.ImageUrl = photo.Url;
                member.User.ImageUrl = photo.Url;
            }

            member.Photos.Add(photo);

            if (await memberRepository.SaveAllAsync()) return photo;
            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());
            if (member == null) return BadRequest("Cannot update member");

            var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);
            if (member.ImageUrl == photo?.Url || photo == null)
            {
                return BadRequest("Cannot set this photo as main");
            }

            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;

            if (await memberRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to set main photo");
        }


        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());
            if (member == null) return BadRequest("Cannot update member");

            var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);
            if (photo == null || photo.Url == member.ImageUrl)
            {
                return BadRequest("Cannot delete this photo");
            }

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            member.Photos.Remove(photo);

            if (await memberRepository.SaveAllAsync()) return Ok();
            return BadRequest("Problem deleting the photo");
        }

    }
}
