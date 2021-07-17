using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Data.IRepositories
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByNameAsync(string name);
        Task<PagedList<MemberDto>> GetMembersAsync(PaginationParams paginationParams);
        Task<MemberDto> GetMemberByNameAsync(string name);
        Task<bool> SaveAllChangesAsync();

    }
}