using System;
using System.Linq;
using MediaSolution.BL.Models; 
using MediaSolution.DAL.Entities;

namespace MediaSolution.BL.Mappers
{
    public class PlaylistModelMapper : ModelMapperBase<PlaylistEntity, PlaylistListModel, PlaylistDetailModel>
    {
        private readonly MediaModelMapper _mediaModelMapper;

        public PlaylistModelMapper(MediaModelMapper mediaModelMapper)
        {
            _mediaModelMapper = mediaModelMapper;
        }

        public override PlaylistListModel MapToListModel(PlaylistEntity? entity)
            => entity is null
                ? PlaylistListModel.Empty
                : new PlaylistListModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    CoverImage = entity.CoverImage,
                    Favorite = entity.Favorite,
                    MediaIds = entity.Media.Select(m => m.MediaId).ToList()
                };

        public override PlaylistDetailModel MapToDetailModel(PlaylistEntity entity)
            => entity is null
                ? PlaylistDetailModel.Empty
                : new PlaylistDetailModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    Favorite = entity.Favorite,
                    CoverImage = entity.CoverImage,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt,
                   
                };

        public override PlaylistEntity MapToEntity(PlaylistDetailModel model)
            => new PlaylistEntity
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Favorite = model.Favorite,
                CoverImage = model.CoverImage,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
    }
}
