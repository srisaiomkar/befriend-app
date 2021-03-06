using System;
using System.Collections.Generic;
using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
         public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NickName { get; set; }
        public string Bio { get; set; }
        public DateTime AccountCreated { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Interests { get; set; }
        public string LookingFor { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Like> LikedByUsers { get; set; }
        public ICollection<Like> LikedUsers { get; set; }
    }
}