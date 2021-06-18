using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Data.IRepositories
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByNameAsync(string name);
        Task<bool> SaveAllChangesAsync();

    }
}