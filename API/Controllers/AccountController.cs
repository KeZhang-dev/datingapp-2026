using System.Security.Cryptography;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using API.Extensions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
    {
        //api/account/register
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (await EmailExists(registerDto.Email))
                return BadRequest("Email is already taken.");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email.ToLower(),
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user.ToDto(tokenService);
        }

        // 这里意思就是检查用户输入的电子邮件地址是否已经存在于数据库中，如果存在就返回 true，否则返回 false。
        private async Task<bool> EmailExists(string email)
        {
            return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        }







        //api/account/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            // 这里的 Email() 方法是 LoginDTO 类中的一个方法，返回用户输入的电子邮件地址。
            var user = await context.Users.SingleOrDefaultAsync(x => x.Email == loginDTO.Email);

            // 如果没有找到用户，返回 401 Unauthorized 响应，并提供一个消息说明原因。
            if (user == null) return Unauthorized("Invalid email.");

            // 然后用HMACSHA512来验证用户输入的密码是否正确
            using var hamc = new HMACSHA512(user.PasswordSalt);

            // 然后用计算机生成的哈希值与数据库中的哈希值比较
            var computedHash = hamc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginDTO.Password));

            // 这里用for来比较，如果哈希值不匹配，就告诉用户密码无效，相反，如果哈希值匹配，就返回用户信息。
            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password.");
            }

            return user.ToDto(tokenService);
        }




    }
}
