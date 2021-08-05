using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data.IRepositories;
using API.DTOs;
using API.Entities;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> LikeUser(string username){
            var likedUser = await _userRepository.GetUserByNameAsync(username);
            if(likedUser == null)
                return BadRequest("User does not exist");
            var currentUserId = User.GetUserId();
            var currentUser = await _likesRepository.GetUserWithLikes(currentUserId);
            if(currentUser.UserName == username)
                return BadRequest("You cannot like yourself :) ");

            if(await _likesRepository.CheckLike(currentUserId,likedUser.Id))
                return BadRequest("User already liked");
            
            var like = new Like{
                LikedByUserId = currentUserId,
                LikedUserId = likedUser.Id
            };

            currentUser.LikedUsers.Add(like);

            if(await _userRepository.SaveAllChangesAsync())
                return Ok();
            return BadRequest("An error has occured");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams){
            likesParams.UserId = User.GetUserId();
            var likes = await _likesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(likesParams.PageNumber,likesParams.ItemsPerPage,
                likes.TotalPages,likes.TotalItems);
            return Ok(likes);
        }
    }
}