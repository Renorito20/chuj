using MediaSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaSolution.DAL.Seeds;
public static class MediaSeeds
{
    public static readonly MediaEntity TestSong = new()
    {
        Id = Guid.Parse("1a82f3d5-8f83-42de-9bb2-20b13ce2b531"),
        Name = "Test Song",
        Duration = TimeSpan.FromMinutes(3),
        SizeInBytes = 100,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        Authors = "Artist Jozo",
        Genres = "Pop",
        Path = "path..."
    };
    public static readonly MediaEntity TestSong1 = new()
    {
        Id = Guid.Parse("4a82f3d5-8f83-42de-9bb2-20b13ce2b534"),
        Name = "Test Song x",
        Duration = TimeSpan.FromMinutes(3),
        SizeInBytes = 100,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        Authors = "Artist Fero",
        Genres = "Punk",
        Path = "path..."
    };

    public static readonly MediaEntity TestSong2 = new()
    {
        Id = Guid.Parse("2a82f3d5-8f83-42de-9bb2-20b13ce2b532"),
        Name = "Test Song 2",
        Duration = TimeSpan.FromMinutes(5),
        SizeInBytes = 420,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        Authors = "Artist Matej",
        Genres = "Rock",
        Path = "path..."
    };

    public static readonly MediaEntity TestSong3 = new()
    {
        Id = Guid.Parse("3a82f3d5-8f83-42de-9bb2-20b13ce2b533"),
        Name = "Test Song 3",
        Duration = TimeSpan.FromMinutes(4),
        SizeInBytes = 300,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        Authors = "Artist Ado",
        Genres = "Electronic",
        Path = "path..."
    };

    public static DbContext SeedMedia(this DbContext dbx)
    {
        dbx.Set<MediaEntity>().AddRange(
            TestSong, TestSong1, TestSong2, TestSong3
        );
        return dbx;
    }
}