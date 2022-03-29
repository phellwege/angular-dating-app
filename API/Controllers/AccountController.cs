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
        public async Task<ActionResult<AppUser>> Register(RegisterDto RegisterDto) 
        {   
            if (await UserExists(RegisterDto.UserName)) return BadRequest("Username is Unavailable");

            using var hmac = new HMACSHA512();
            
            var user = new AppUser
            {
                UserName = RegisterDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(RegisterDto.Password)),
                PasswordSalt = hmac.Key
            };
            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();

            return user;
        }
        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto) 
        {
            var user = await this.context.Users
                .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if (user == null) return Unauthorized("Invalid Username");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i = 0; i < ComputeHash.Length; i++)
            {
                if(ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return user;
        }
        private async Task<bool> UserExists(string UserName)
        {
            return await this.context.Users.AnyAsync(user => user.UserName == UserName.ToLower());
        }
    }
}