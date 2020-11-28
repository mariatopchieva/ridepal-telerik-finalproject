using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RidePal.Data.Configurations;
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
        public DbSet<PlaylistFavorite> Favorites { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public DbSet<PlaylistGenre> PlaylistGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlaylistFavoriteConfig());
            modelBuilder.ApplyConfiguration(new PlaylistGenreConfig());
            modelBuilder.ApplyConfiguration(new PlaylistTrackConfig());

            modelBuilder. Entity<Role>().HasData(
                new Role() { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new Role() { Id = 2, Name = "User", NormalizedName = "USER" }
                );
            
            var hasher = new PasswordHasher<User>();

            User admin = new User
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "ADMIN@RIDEPAL.COM",
                Email = "admin@ridepal.com",
                NormalizedEmail = "ADMIN@RIDEPAL.COM",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            admin.PasswordHash = hasher.HashPassword(admin, "admin123");

            modelBuilder.Entity<User>().HasData(admin);

            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    RoleId = 1,
                    UserId = admin.Id
                });

            User user = new User
            {
                Id = 2,
                UserName = "user",
                NormalizedUserName = "USER@RIDEPAL.COM",
                Email = "user@ridepal.com",
                NormalizedEmail = "USER@RIDEPAL.COM",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            user.PasswordHash = hasher.HashPassword(user, "user123");

            modelBuilder.Entity<User>().HasData(user);

            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    RoleId = 2,
                    UserId = user.Id
                });

            modelBuilder.Entity<Playlist>(entity => {
                entity.HasIndex(e => e.Title).IsUnique(true);
            });

            //modelBuilder.Entity<PlaylistFavorite>().HasOne(pf => pf.Playlist)
            //    .WithMany(playlist => playlist.Favorites)
            //    .HasForeignKey(pf => pf.PlaylistId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<PlaylistFavorite>().HasOne(pf => pf.User)
            //    .WithMany(User => User.Favorites)
            //    .HasForeignKey(pf =>  pf.UserId)
            //    .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
