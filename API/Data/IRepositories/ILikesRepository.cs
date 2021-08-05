using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Data.IRepositories
{
    public interface ILikesRepository
    {
        Task<bool> CheckLike(int likedByUserId, int likedUserId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
        Task<AppUser> GetUserWithLikes(int userId);
    }
}