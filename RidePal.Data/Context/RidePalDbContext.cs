using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Context
{
    public class RidePalDbContext : IdentityDbContext<User, Role, int>
    {
        public RidePalDbContext()
        {

        }

        public RidePalDbContext(DbContextOptions<RidePalDbContext> options) : base(options)
        {

        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Track> Tracks { get; set; }

        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public DbSet<PlaylistGenre> PlaylistGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder. Entity<Role>().HasData(
                new Role() { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new Role() { Id = 2, Name = "User", NormalizedName = "USER" }
                );
            
            var hasher = new PasswordHasher<User>();

            var admin = new User
            {
                Id = 1,
                FirstName = "Maria",
                LastName = "Topchieva",
                UserName = "Maria",
                NormalizedUserName = "MARIA",
                Email = "maria_topchieva@abv.bg",
                NormalizedEmail = "MARIA_TOPCHIEVA@ABV.BG",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Registration87!");
            modelBuilder.Entity<User>().HasData(admin);
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>()
                {
                    RoleId = 1,
                    UserId = admin.Id
                }
                );

            var user = new User
            {
                Id = 2,
                FirstName = "Maria",
                LastName = "Topchieva",
                UserName = "MariaTop",
                NormalizedUserName = "MARIATOP",
                Email = "maria.topchieva@abv.bg",
                NormalizedEmail = "MARIA.TOPCHIEVA@ABV.BG",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            user.PasswordHash = hasher.HashPassword(user, "Registration87!");
            modelBuilder.Entity<User>().HasData(user);
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>()
                {
                    RoleId = 2,
                    UserId = user.Id
                }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
