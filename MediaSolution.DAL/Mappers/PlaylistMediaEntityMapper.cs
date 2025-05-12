using MediaSolution.DAL.Entities;

namespace MediaSolution.DAL.Mappers;

public class PlaylistMediaEntityMapper: IEntityMapper<PlaylistMediaEntity>
{
    public void MapToExistingEntity(PlaylistMediaEntity existingEntity, PlaylistMediaEntity newEntity)
    {
        existingEntity.PlaylistId = newEntity.PlaylistId;
        existingEntity.MediaId = newEntity.MediaId;
        existingEntity.Order = newEntity.Order;
    }
}