using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.IRepositories;
using API.DTOs;
using API.Entities;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();
            query = query.Where(user => user.Gender == userParams.Gender);
            query = query.Where(user => user.UserName != userParams.CurrentUserName);

            //-1 year because say if maxAge is 30 if a person born on feb and current month is march the person would still be "30 running"
            // Added 1 day because
            /*
                if max age is 30
                and a person born on mar 1 2001 and today is mar 1 2032
                AddYears(-userParams.maxAge -1) will make it mar 1 2001
                the person's age would be 31 but max age is 30  

            */
            var minDob = DateTime.Today.AddYears(-userParams.maxAge -1).AddDays(1);
            var maxDob = DateTime.Today.AddYears(-userParams.minAge);
            query = query.Where(user => user.DateOfBirth >=minDob && user.DateOfBirth <= maxDob);

            switch(userParams.OrderBy){
                case "accountCreated":
                    query = query.OrderByDescending(u => u.AccountCreated);
                    break;
                default:
                    query = query.OrderByDescending(u => u.LastActive);
                    break;
            }

            // With AsNoTracking, Entity Framework performs no additional processing or storage of the entities which are returned by the query
            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .AsNoTracking(),userParams.PageNumber,userParams.ItemsPerPage);
        }

        public async Task<MemberDto> GetMemberByNameAsync(string name)
        {
            return await _context.Users.Where(user =>user.UserName == name)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByNameAsync(string username)
        {
            return await _context.Users
            .Include(user => user.Photos)
            .SingleOrDefaultAsync(user => user.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
            .Include(user => user.Photos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}