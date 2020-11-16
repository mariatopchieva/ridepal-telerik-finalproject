using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Configurations
{
    public class PlaylistTrackConfig : IEntityTypeConfiguration<PlaylistTrack>
    {
        public void Configure(EntityTypeBuilder<PlaylistTrack> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(pf => pf.Playlist)
                .WithMany(playlist => playlist.Tracks)
                .HasForeignKey(pf => pf.PlaylistId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pf => pf.Track)
                .WithMany(track => track.Playlists)
                .HasForeignKey(pf => pf.TrackId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
