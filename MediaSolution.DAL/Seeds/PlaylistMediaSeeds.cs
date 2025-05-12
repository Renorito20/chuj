using MediaSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaSolution.DAL.Seeds;

public static class PlaylistMediaSeeds
{
    public static readonly PlaylistMediaEntity WorkoutJamsLink = new()
    {
        Id = Guid.Parse("b92a8b9e-b2c5-469d-83b0-dce44a21b72a"),
        MediaId = MediaSeeds.TestSong.Id,
        PlaylistId = PlaylistSeeds.WorkoutJams.Id,
        Order = 1
    };
    public static readonly PlaylistMediaEntity WorkoutJamsLink2 = new()
    {
        Id = Guid.Parse("e92a8b9e-b2c5-469d-83b0-dce44a21b72d"),
        MediaId = MediaSeeds.TestSong1.Id, // Make sure this exists in MediaSeeds
        PlaylistId = PlaylistSeeds.WorkoutJams.Id, // Adding to WorkoutJams playlist
        Order = 2 // Setting order to 2 since there's already one media in this playlist
    };

    public static readonly PlaylistMediaEntity ChillVibesLink = new()
    {
        Id = Guid.Parse("c82a8b9e-b2c5-469d-83b0-dce44a21b72b"),
        MediaId = MediaSeeds.TestSong2.Id,
        PlaylistId = PlaylistSeeds.ChillVibes.Id,
        Order = 1
    };

    public static readonly PlaylistMediaEntity RoadTripLink = new()
    {
        Id = Guid.Parse("d92a8b9e-b2c5-469d-83b0-dce44a21b72c"),
        MediaId = MediaSeeds.TestSong3.Id,
        PlaylistId = PlaylistSeeds.RoadTrip.Id,
        Order = 1
    };

    public static DbContext SeedPlaylistMedia(this DbContext dbx)
    {
        dbx.Set<PlaylistMediaEntity>().AddRange(
            WorkoutJamsLink,
            WorkoutJamsLink2,
            ChillVibesLink,
            RoadTripLink
        );
        return dbx;
    }
}