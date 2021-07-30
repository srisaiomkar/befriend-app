using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Data.IRepositories;
using API.DTOs;
using API.Entities;
using API.Extensions;
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
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var user = await _userRepository.GetUserByIdAsync(User.GetUserId());
            userParams.CurrentUserName = user.UserName;
            if(string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = user.Gender == "male"? "female" : "male";

            var users = await _userRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users.PageNumber,users.ItemsPerPage,
                users.TotalPages,users.TotalItems);
            return Ok(users);
        }

        [HttpGet]
        [Route("{username}",Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberByNameAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UpdateMemberDto updateMemberDto){
            var user = await _userRepository.GetUserByIdAsync(User.GetUserId());
            _mapper.Map(updateMemberDto,user);
            _userRepository.Update(user);

            if(await _userRepository.SaveAllChangesAsync()) return NoContent();
            return BadRequest("Updating user failed");
        }

        [HttpPost("photo")]
        public async Task<ActionResult<PhotoDto>> UploadPhoto(IFormFile file){
            var user = await _userRepository.GetUserByNameAsync(User.GetUsername());
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
            var user = await _userRepository.GetUserByIdAsync(User.GetUserId());

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

        [HttpDelete("photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId){
            var user = await _userRepository.GetUserByIdAsync(User.GetUserId());

            var photoToDelete = user.Photos.FirstOrDefault(photo=> photo.Id == photoId);

            if(photoToDelete == null)   
                return NotFound("photo does not exist");
            if(photoToDelete.IsMain)
                return BadRequest("Cannot delete main photo");
            if(photoToDelete.PublicId != null){
                var result = await _photoService.DeletePhotoAsync(photoToDelete.PublicId);
                if(result.Error != null)
                    return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photoToDelete);
            if(await _userRepository.SaveAllChangesAsync())
                return NoContent();
            return BadRequest("Problem deleting photo");
        }
    }
}