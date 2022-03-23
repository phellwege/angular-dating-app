using API.Data;
using API.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using API.DTOs;

namespace API.Controllers

{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        public AccountController(DataContext context) {
            this.context = context;
        }
        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto)
        {
            using var hmac = new HMACSHA512();
            
            var user = new AppUser
            {
                UserName = username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };
            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();

            return user;
        }
    }
}