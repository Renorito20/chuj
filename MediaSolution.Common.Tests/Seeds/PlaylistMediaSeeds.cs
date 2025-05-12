using MediaSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaSolution.Common.Tests.Seeds;

public static class PlaylistMediaSeeds
{
    public static readonly PlaylistMediaEntity PlaylistMediaLayla = new()
    {
        Id = Guid.Parse("fabde0cd-eefe-443f-baf6-3d96cc2cbf24"),
        Order = 1,
        PlaylistId = PlaylistSeeds.EricClaptonPlaylist.Id,
        Playlist = PlaylistSeeds.EricClaptonPlaylist,
        MediaId = MediaSeeds.Layla.Id,
        Media = MediaSeeds.Layla
    };

    public static readonly PlaylistMediaEntity PlaylistMediaTearsInHeaven = new()
    {
        Id = Guid.Parse("fabde0cd-eefe-443f-baf6-3d96cc2cbf23"),
        Order = 2,
        PlaylistId = PlaylistSeeds.EricClaptonPlaylist.Id,
        Playlist = PlaylistSeeds.EricClaptonPlaylist,
        MediaId = MediaSeeds.TearsInHeaven.Id,
        Media = MediaSeeds.TearsInHeaven
    };

    // Empty PlaylistMedia Entity
    public static readonly PlaylistMediaEntity EmptyPlaylistMedia = new()
    {
        Id = default,
        Order = default,
        PlaylistId = default,
        Playlist = null,
        MediaId = default,
        Media = null
    };

    // PlaylistMedia for Update
    public static readonly PlaylistMediaEntity PlaylistMediaUpdate = PlaylistMediaLayla with
    {
        Id = Guid.Parse("fabde0cd-eefe-443f-baf6-3d96cc2cbf22"),
        Order = 3
    };

    // PlaylistMedia for Delete
    public static readonly PlaylistMediaEntity PlaylistMediaDelete = PlaylistMediaTearsInHeaven with
    {
        Id = Guid.Parse("fabde0cd-eefe-443f-baf6-3d96cc2cbf21")
    };

    public static DbContext SeedPlaylistMedia(this DbContext dbx)
    {
        dbx.Set<PlaylistMediaEntity>().AddRange(PlaylistMediaLayla, PlaylistMediaTearsInHeaven, PlaylistMediaUpdate, PlaylistMediaDelete);
        return dbx;
    }
}