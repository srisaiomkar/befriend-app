using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.IRepositories;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;

        public LikesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckLike(int likedByUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(likedByUserId,likedUserId) ==null? false: true;
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(user => user.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(likesParams.Predicate == "liked"){
                likes = likes.Where(like => like.LikedByUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }
            else if(likesParams.Predicate == "likedBy"){
                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedByUser);
            }

            var query = users.Select(user => new LikeDto{
                Id = user.Id,
                UserName = user.UserName,
                Age = user.DateOfBirth.CalculateAge(),
                City = user.City,
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
                NickName = user.NickName,
                MainPhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain).Url
            }).AsQueryable();

            return await PagedList<LikeDto>.CreateAsync(query,likesParams.PageNumber,likesParams.ItemsPerPage);
        }
    

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
            .Include(user => user.LikedUsers)
            .FirstOrDefaultAsync(user => user.Id == userId);
        }
    }
}