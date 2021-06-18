using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext dataContext){
            if(await dataContext.Users.AnyAsync()) return;
            var UsersJson = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(UsersJson);

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();

                using var hmac = new HMACSHA512();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("qqq"));
                user.PasswordSalt = hmac.Key;

                dataContext.Users.Add(user);
            }
            await dataContext.SaveChangesAsync();
        }
    }
}