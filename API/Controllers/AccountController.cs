using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        
        private async Task<bool> UserExists(string username){
            return await _context.Users.AnyAsync(user => user.UserName == username.ToLower());
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserRegisterDto userRegisterDto){
            if(await UserExists(userRegisterDto.Username))
                return BadRequest("Username already exists");
            using var hmac = new HMACSHA512();
            var user = new AppUser{
                UserName = userRegisterDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto userLoginDto){
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == userLoginDto.Username);
            
            if(user == null)
                return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var loginPwdHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userLoginDto.Password));

            for(int i =0;i<loginPwdHash.Length;i++){
                if(loginPwdHash[i] != user.PasswordHash[i])
                    return Unauthorized("Incorrect Password");
            }
            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}