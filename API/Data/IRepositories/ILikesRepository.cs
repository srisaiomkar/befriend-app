using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Data.IRepositories
{
    public interface ILikesRepository
    {
        Task<bool> CheckLike(int likedByUserId, int likedUserId);
        Task<IEnumerable<LikeDto>> GetUserLikes(int userId, string predicate);
        Task<AppUser> GetUserWithLikes(int userId);
    }
}