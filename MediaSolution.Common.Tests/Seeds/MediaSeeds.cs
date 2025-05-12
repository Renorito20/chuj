using MediaSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaSolution.Common.Tests.Seeds;

public static class MediaSeeds
{
    public static readonly MediaEntity EmptyMedia = new()
    {
        Id = default,
        Name = default!,
        Duration = default,
        SizeInBytes = default,
        CreatedAt = default,
        UpdatedAt = default,
        Authors = null,
        Genres = null,
        Path = "default.mp3",
    };

    public static readonly MediaEntity Layla = new()
    {
        Id = Guid.Parse(input: "fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
        Name = "Layla",
        Duration = TimeSpan.FromMinutes(7),
        SizeInBytes = 8000000,
        CreatedAt = new DateTime(2025, 3, 9, 12, 0, 0, DateTimeKind.Utc),
        UpdatedAt = new DateTime(2025, 3, 9, 12, 0, 0, DateTimeKind.Utc),

        Authors = "Eric Clapton",
        Genres = "Rock",
        Path = "/media/layla.mp3",
    };

    public static MediaEntity TearsInHeaven = new()
    {
        Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC2E"),
        Name = "Tears in Heaven",
        Duration = TimeSpan.FromMinutes(5),
        SizeInBytes = 6000000,
        CreatedAt = new DateTime(2025, 3, 9, 12, 0, 0, DateTimeKind.Utc),
        UpdatedAt = new DateTime(2025, 3, 9, 12, 0, 0, DateTimeKind.Utc),

        Authors = "Eric Clapton",
        Genres = "Acoustic",
        Path = "/media/tears_in_heaven.mp3",
    };
    
    public static MediaEntity BeforeYouAccuseMe = new()
    {
        Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC29"),
        Name = "Before you accuse me",
        Duration = TimeSpan.FromMinutes(4),
        SizeInBytes = 4000000,
        CreatedAt = new DateTime(2025, 3, 9, 12, 0, 0, DateTimeKind.Utc),
        UpdatedAt = new DateTime(2025, 3, 9, 12, 0, 0, DateTimeKind.Utc),

        Authors = "Eric Clapton",
        Genres = "Acoustic",
        Path = "/media/tears_in_heaven.mp3",
    };

    // Update and Delete versions
    public static readonly MediaEntity MediaUpdate = Layla with { Id = Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F67868") }; // Valid Guid
    public static readonly MediaEntity MediaDelete = Layla with { Id = Guid.Parse("5DCA4CEA-B8A8-4C86-A0B3-FFB78FBA1A09") }; // Valid Guid

    public static DbContext SeedMedia(this DbContext dbx)
    {
        dbx.Set<MediaEntity>().AddRange(Layla, TearsInHeaven, BeforeYouAccuseMe, MediaUpdate, MediaDelete);
        return dbx;
    }
}