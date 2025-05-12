namespace MediaSolution.DAL;

using MediaSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class MediaSolutionDbContext(DbContextOptions contextOptions) : DbContext(contextOptions)
{
    public DbSet<MediaEntity> MediaEntities => Set<MediaEntity>();
    public DbSet<PlaylistEntity> PlaylistEntities => Set<PlaylistEntity>();
    public DbSet<PlaylistMediaEntity> PlaylistMediaEntities => Set<PlaylistMediaEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PlaylistMediaEntity>()
            .HasOne(pm => pm.Media)
            .WithMany()
            .HasForeignKey(pm => pm.MediaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PlaylistEntity>()
            .HasMany(i => i.Media)
            .WithOne(i => i.Playlist)
            .OnDelete(DeleteBehavior.Cascade);
    }
}