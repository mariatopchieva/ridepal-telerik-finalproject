using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Configurations
{
    public class PlaylistFavoriteConfig : IEntityTypeConfiguration<PlaylistFavorite>
    {
        public void Configure(EntityTypeBuilder<PlaylistFavorite> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(pf => pf.Playlist)
                .WithMany(playlist => playlist.Favorites)
                .HasForeignKey(pf => pf.PlaylistId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pf => pf.User)
                .WithMany(user => user.Favorites)
                .HasForeignKey(pf => pf.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
