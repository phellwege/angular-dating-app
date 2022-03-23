using API.Data;
using API.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers

{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        public AccountController(DataContext context) {
            this.context = context;
        }
        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto RegisterDto) {}
        {   
            using var hmac = new HMACSHA512();
            
            var user = new AppUser
            {
                UserName = RegisterDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(RegisterDto.Password)),
                PasswordSalt = hmac.Key
            };
            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();

            return user;
        }
        private async Task<bool> UserExists(string username)
        {
            return await this.context.Users.AnyAsync(user => user.UserName == username.ToLower());
        }
    }
}