using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Data.IRepositories;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper,
        IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var usersToReturn = await _userRepository.GetMembersAsync();
            return Ok(usersToReturn);
        }

        [HttpGet]
        [Route("{username}",Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMembersByNameAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UpdateMemberDto updateMemberDto){
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByNameAsync(username);
            _mapper.Map(updateMemberDto,user);
            _userRepository.Update(user);

            if(await _userRepository.SaveAllChangesAsync()) return NoContent();
            return BadRequest("Updating user failed");
        }

        [HttpPost("photo")]
        public async Task<ActionResult<PhotoDto>> UploadPhoto(IFormFile file){
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByNameAsync(username);
            ImageUploadResult uploadResult = await _photoService.UploadPhotoAsync(file);

            if(uploadResult.Error != null)
                return BadRequest(uploadResult.Error.Message);
            
            Photo photo = new Photo{
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };

            if(user.Photos.Count == 0){
                photo.IsMain = true;
            }

            user.Photos.Add(photo);
            if(await _userRepository.SaveAllChangesAsync()){
                return CreatedAtRoute("GetUser", new {username = user.UserName},
                _mapper.Map<PhotoDto>(photo));
            }
            return BadRequest("Problem adding photo");
        }

        [HttpPut("main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId){
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByNameAsync(username);

            var currentMainPhoto = user.Photos.FirstOrDefault(photo => photo.IsMain == true);
            var newMainPhoto = user.Photos.FirstOrDefault(photo => photo.Id == photoId);
            if(currentMainPhoto == newMainPhoto)
                return BadRequest("This is already the main photo");
            if(currentMainPhoto != null){
                currentMainPhoto.IsMain = false;
            }
            newMainPhoto.IsMain = true;
            if(await _userRepository.SaveAllChangesAsync())
                return NoContent();
            return BadRequest("Failed to make the photo main");
        }
    }
}