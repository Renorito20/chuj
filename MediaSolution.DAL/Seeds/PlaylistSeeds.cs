using MediaSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace MediaSolution.DAL.Seeds;

public static class PlaylistSeeds
{
    public static readonly PlaylistEntity WorkoutJams = new()
    {
        Id = Guid.Parse("07ce9296-3cf0-40d1-bf96-8a49e342a1e9"),
        Name = "Workout Jams",
        Description = "High-energy tracks to power your workout",
        Favorite = true,
        CoverImage = "https://images.unsplash.com/photo-1746469435655-00d7340ec1c4?q=80&w=1471&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
        CreatedAt = DateTime.UtcNow.AddDays(-30),
        UpdatedAt = DateTime.UtcNow.AddDays(-7)
    };

    public static readonly PlaylistEntity ChillVibes = new()
    {
        Id = Guid.Parse("18df9367-2a5f-4b3c-8d1e-0f9a8b7c6d5e"),
        Name = "Chill Vibes",
        Description = "Relaxing tunes for winding down",
        Favorite = true,
        CoverImage = "https://images.unsplash.com/photo-1743275532247-98709c8c3f5b?q=80&w=1374&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
        CreatedAt = DateTime.UtcNow.AddDays(-25),
        UpdatedAt = DateTime.UtcNow.AddDays(-3)
    };

    public static readonly PlaylistEntity RoadTrip = new()
    {
        Id = Guid.Parse("29ed0476-3b8f-4c2a-9d1e-5f6a7b8c9d0e"),
        Name = "Road Trip",
        Description = "The perfect soundtrack for your journey",
        Favorite = false,
        CoverImage = "https://images.unsplash.com/photo-1746311460525-31a29b35f4c6?q=80&w=1426&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
        CreatedAt = DateTime.UtcNow.AddDays(-20),
        UpdatedAt = DateTime.UtcNow.AddDays(-1)
    };

    

    public static DbContext SeedPlaylists(this DbContext dbx)
    {
        dbx.Set<PlaylistEntity>().AddRange(
            WorkoutJams with { Media = new List<PlaylistMediaEntity>() },
            ChillVibes with { Media = new List<PlaylistMediaEntity>() },
            RoadTrip with { Media = new List<PlaylistMediaEntity>() }
        );
        return dbx;
    }
}