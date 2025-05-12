using MediaSolution.BL.Models;
using MediaSolution.DAL.Entities;

namespace MediaSolution.BL.Mappers
{
    public class PlaylistMediaModelMapper : ModelMapperBase<PlaylistMediaEntity, PlaylistMediaList, PlaylistMediaDetail>
    {
        public override PlaylistMediaList MapToListModel(PlaylistMediaEntity? entity)
            => entity is null
                ? PlaylistMediaList.Empty
                : new PlaylistMediaList
                {
                    Id = entity.Id,
                    Order = entity.Order,
                    MediaId = entity.MediaId
                };

        public override PlaylistMediaDetail MapToDetailModel(PlaylistMediaEntity? entity)
            => entity is null
                ? PlaylistMediaDetail.Empty
                : new PlaylistMediaDetail
                {
                    Id = entity.Id,
                    Order = entity.Order,
                    MediaId = entity.MediaId,
                    PlaylistId = entity.PlaylistId
                };

        public PlaylistMediaList MapToListModel(PlaylistMediaDetail detailModel)
            => new PlaylistMediaList
            {
                Id = detailModel.Id,
                Order = detailModel.Order,
                MediaId = detailModel.MediaId
            };

        public override PlaylistMediaEntity MapToEntity(PlaylistMediaDetail model)
            => throw new NotImplementedException("This method is unsupported. Use the overload with explicit playlistId.");

        public PlaylistMediaEntity MapToEntity(PlaylistMediaDetail model, Guid playlistId)
            => new PlaylistMediaEntity
            {
                Id = model.Id,
                Order = model.Order,
                MediaId = model.MediaId,
                PlaylistId = playlistId
            };

        public PlaylistMediaEntity MapToEntity(PlaylistMediaList model, Guid playlistId)
            => new PlaylistMediaEntity
            {
                Id = model.Id,
                Order = model.Order,
                MediaId = model.MediaId,
                PlaylistId = playlistId
            };
    }
}
