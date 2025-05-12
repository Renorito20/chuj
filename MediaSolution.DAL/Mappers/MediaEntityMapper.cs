using MediaSolution.DAL.Entities;

namespace MediaSolution.DAL.Mappers;

public class MediaEntityMapper: IEntityMapper<MediaEntity>
{
    public void MapToExistingEntity(MediaEntity existingEntity, MediaEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.Duration = newEntity.Duration;
        existingEntity.SizeInBytes = newEntity.SizeInBytes;
        
        existingEntity.CreatedAt = newEntity.CreatedAt;
        existingEntity.UpdatedAt = newEntity.UpdatedAt;
        
        existingEntity.Authors = newEntity.Authors;
        existingEntity.Genres = newEntity.Genres;
        existingEntity.Path = newEntity.Path;
    }

}