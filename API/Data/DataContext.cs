using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);

            builder.Entity<Like>()
                .HasKey(l => new {l.LikedByUserId,l.LikedUserId});

            builder.Entity<Like>()
                .HasOne(l => l.LikedByUser)
                .WithMany(u => u.LikedUsers)
                .HasForeignKey(l => l.LikedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Like>()
                .HasOne(l => l.LikedUser)
                .WithMany(u => u.LikedByUsers)
                .HasForeignKey(l => l.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}