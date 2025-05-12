using MediaSolution.DAL.Entities;

namespace MediaSolution.DAL.Mappers;

public class PlaylistEntityMapper: IEntityMapper<PlaylistEntity>
{
    public void MapToExistingEntity(PlaylistEntity existingEntity, PlaylistEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.Description = newEntity.Description;
        existingEntity.Favorite = newEntity.Favorite;
        existingEntity.CoverImage = newEntity.CoverImage;
        
        existingEntity.CreatedAt = newEntity.CreatedAt;
        existingEntity.UpdatedAt = newEntity.UpdatedAt;
    }
}