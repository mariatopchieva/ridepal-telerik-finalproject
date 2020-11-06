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
            //already seeded.
            //modelBuilder.Seed();

            base.OnModelCreating(modelBuilder);
        }



        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=RidePal3;Trusted_Connection=True;");

        //}

    }
}
