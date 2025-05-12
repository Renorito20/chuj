using System;
using MediaSolution.BL.Mappers;
using MediaSolution.BL.Models;
using MediaSolution.DAL.Entities;
using Xunit;

namespace MediaSolution.BL.Tests.Mappers
{
    public class PlaylistMediaModelMapperTests
    {
        private readonly PlaylistMediaModelMapper _mapper = new PlaylistMediaModelMapper();
        private readonly Guid playlistId = Guid.NewGuid();

        [Fact]
        public void MapToListModel_WithValidEntity_ReturnsPlaylistMediaList()
        {
            // Arrange
            var entity = new PlaylistMediaEntity
            {
                Id = Guid.NewGuid(),
                Order = 1,
                MediaId = Guid.NewGuid(),
                PlaylistId = playlistId
            };

            // Act
            var listModel = _mapper.MapToListModel(entity);

            // Assert
            Assert.Equal(entity.Id, listModel.Id);
            Assert.Equal(entity.Order, listModel.Order);
            Assert.Equal(entity.MediaId, listModel.MediaId);
        }

        [Fact]
        public void MapToDetailModel_WithValidEntity_ReturnsPlaylistMediaDetail()
        {
            // Arrange
            var entity = new PlaylistMediaEntity
            {
                Id = Guid.NewGuid(),
                Order = 2,
                MediaId = Guid.NewGuid(),
                PlaylistId = playlistId
            };

            // Act
            var detailModel = _mapper.MapToDetailModel(entity);

            // Assert
            Assert.Equal(entity.Id, detailModel.Id);
            Assert.Equal(entity.Order, detailModel.Order);
            Assert.Equal(entity.MediaId, detailModel.MediaId);
            Assert.Equal(entity.PlaylistId, detailModel.PlaylistId);
        }

        [Fact]
        public void MapToEntity_WithDetailModelAndPlaylistId_ReturnsPlaylistMediaEntity()
        {
            // Arrange
            var detailModel = new PlaylistMediaDetail
            {
                Id = Guid.NewGuid(),
                Order = 3,
                MediaId = Guid.NewGuid(),
                PlaylistId = playlistId // This value will be overridden by the explicit playlistId parameter
            };

            // Act
            var entity = _mapper.MapToEntity(detailModel, playlistId);

            // Assert
            Assert.Equal(detailModel.Id, entity.Id);
            Assert.Equal(detailModel.Order, entity.Order);
            Assert.Equal(detailModel.MediaId, entity.MediaId);
            Assert.Equal(playlistId, entity.PlaylistId);
        }

        [Fact]
        public void MapToEntity_WithListModelAndPlaylistId_ReturnsPlaylistMediaEntity()
        {
            // Arrange
            var listModel = new PlaylistMediaList
            {
                Id = Guid.NewGuid(),
                Order = 4,
                MediaId = Guid.NewGuid()
            };

            // Act
            var entity = _mapper.MapToEntity(listModel, playlistId);

            // Assert
            Assert.Equal(listModel.Id, entity.Id);
            Assert.Equal(listModel.Order, entity.Order);
            Assert.Equal(listModel.MediaId, entity.MediaId);
            Assert.Equal(playlistId, entity.PlaylistId);
        }
    }
}
