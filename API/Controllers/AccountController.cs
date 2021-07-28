using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _tokenService = tokenService;
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.UserName == username.ToLower());
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserRegisterDto userRegisterDto)
        {
            if (await UserExists(userRegisterDto.Username))
                return BadRequest("Username already exists");

            var user = _mapper.Map<AppUser>(userRegisterDto);
            using var hmac = new HMACSHA512();

            user.UserName = userRegisterDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDto.Password));
            user.PasswordSalt = hmac.Key;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                Nickname = user.NickName,
                Gender = user.Gender
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto userLoginDto)
        {
            var user = await _context.Users
            .Include(user => user.Photos)
            .SingleOrDefaultAsync(x => x.UserName == userLoginDto.Username);

            if (user == null)
                return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var loginPwdHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userLoginDto.Password));

            for (int i = 0; i < loginPwdHash.Length; i++)
            {
                if (loginPwdHash[i] != user.PasswordHash[i])
                    return Unauthorized("Incorrect Password");
            }
            return new UserDto
            {
                Username = user.UserName,
                Nickname = user.NickName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url,
                Gender = user.Gender
            };
        }
    }
}