using System;
using MediaSolution.BL.Models;  
using MediaSolution.DAL.Entities;

namespace MediaSolution.BL.Mappers
{
    public class MediaModelMapper : ModelMapperBase<MediaEntity, MediaListModel, MediaDetailModel>
    {
        public override MediaListModel MapToListModel(MediaEntity? entity)
            => entity is null
                ? MediaListModel.Empty
                : new MediaListModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Duration = entity.Duration,
                    Authors = entity.Authors,
                    Genres = entity.Genres
                };

        public override MediaDetailModel MapToDetailModel(MediaEntity entity)
            => entity is null
                ? MediaDetailModel.Empty
                : new MediaDetailModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Duration = entity.Duration,
                    SizeInBytes = entity.SizeInBytes,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt,
                    Authors = entity.Authors,
                    Genres = entity.Genres,
                    Path = entity.Path
                };

        public override MediaEntity MapToEntity(MediaDetailModel model)
            => new MediaEntity
            {
                Id = model.Id,
                Name = model.Name,
                Duration = model.Duration,
                SizeInBytes = model.SizeInBytes,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Authors = model.Authors,
                Genres = model.Genres,
                Path = model.Path
            };
    }
}