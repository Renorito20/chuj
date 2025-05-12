using MediaSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaSolution.Common.Tests.Seeds;

public static class PlaylistSeeds
{
    public static readonly PlaylistEntity EmptyPlaylist = new()
    {
        Id = default,
        Name = default!,
        Description = null,
        Favorite = false,
        CoverImage = "default.png",
        CreatedAt = default,
        UpdatedAt = default,
        Media = new List<PlaylistMediaEntity>()
    };

    public static readonly PlaylistEntity EricClaptonPlaylist = new()
    {
        Id = Guid.Parse("4FD824C0-A7D1-48BA-8E7C-4F136CF8BF31"),
        Name = "Eric Clapton",
        Description = "Best songs by Eric Clapton",
        Favorite = true,
        CoverImage = "/images/eric_clapton.jpg",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        Media = new List<PlaylistMediaEntity>()
    };

    // Update and Delete versions
    public static readonly PlaylistEntity PlaylistWithoutMedia = EricClaptonPlaylist with
    {
        Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB89"), Media = new List<PlaylistMediaEntity>()
    };

    public static readonly PlaylistEntity PlaylistUpdate = EricClaptonPlaylist with
    {
        Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB88"), Media = new List<PlaylistMediaEntity>()
    };
    

    public static readonly PlaylistEntity PlaylistDelete = EricClaptonPlaylist with
    {
        Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB87"), Media = new List<PlaylistMediaEntity>()
    };

    public static readonly PlaylistEntity PlaylistMediaUpdate = EricClaptonPlaylist with
    {
        Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB86"), Media = new List<PlaylistMediaEntity>()
    };

    public static readonly PlaylistEntity PlaylistMediaDelete = EricClaptonPlaylist with
    {
        Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB85"), Media = new List<PlaylistMediaEntity>()
    };
    public static readonly PlaylistEntity PlaylistDuration = EricClaptonPlaylist with
    {
        Id = Guid.Parse("F78ED913-E094-4016-9045-3F5BB7F2EB95"), Media = new List<PlaylistMediaEntity>()
    };


    public static DbContext SeedPlaylists(this DbContext dbx)
    {
        dbx.Set<PlaylistEntity>().AddRange(
            EricClaptonPlaylist, 
            PlaylistUpdate, 
            PlaylistDelete, 
            PlaylistWithoutMedia,
            PlaylistMediaUpdate,
            PlaylistMediaDelete,
            PlaylistDuration
            );
        return dbx;
    }
}