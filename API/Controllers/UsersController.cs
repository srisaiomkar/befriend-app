using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Data.IRepositories;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var usersToReturn = await _userRepository.GetMembersAsync();
            return Ok(usersToReturn);
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMembersByNameAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UpdateMemberDto updateMemberDto){
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user =await  _userRepository.GetUserByNameAsync(username);
            _mapper.Map(updateMemberDto,user);
            _userRepository.Update(user);

            if(await _userRepository.SaveAllChangesAsync()) return NoContent();
            return BadRequest("Updating user failed");
        }
    }
}