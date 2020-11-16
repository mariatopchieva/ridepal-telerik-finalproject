using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Configurations
{
    public class PlaylistGenreConfig : IEntityTypeConfiguration<PlaylistGenre>
    {
        public void Configure(EntityTypeBuilder<PlaylistGenre> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(pf => pf.Playlist)
                .WithMany(playlist => playlist.Genres)
                .HasForeignKey(pf => pf.PlaylistId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pf => pf.Genre)
                .WithMany(genre => genre.Playlists)
                .HasForeignKey(pf => pf.GenreId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
