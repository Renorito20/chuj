using System;
using MediaSolution.BL.Mappers;
using MediaSolution.BL.Models;
using MediaSolution.DAL.Entities;
using Xunit;

namespace MediaSolution.BL.Tests.Mappers
{
    public class PlaylistModelMapperTests
    {
        private readonly PlaylistModelMapper _mapper;

        public PlaylistModelMapperTests()
        {
            // Inject a MediaModelMapper dependency for the PlaylistModelMapper
            _mapper = new PlaylistModelMapper(new MediaModelMapper());
        }

        [Fact]
        public void MapToListModel_WithValidEntity_ReturnsPlaylistListModel()
        {
            // Arrange
            var entity = new PlaylistEntity
            {
                Id = Guid.NewGuid(),
                Name = "Playlist 1",
                CoverImage = "cover.jpg",
                Favorite = true
            };

            // Act
            var listModel = _mapper.MapToListModel(entity);

            // Assert
            Assert.Equal(entity.Id, listModel.Id);
            Assert.Equal(entity.Name, listModel.Name);
            Assert.Equal(entity.CoverImage, listModel.CoverImage);
            Assert.Equal(entity.Favorite, listModel.Favorite);
        }

        [Fact]
        public void MapToDetailModel_WithValidEntity_ReturnsPlaylistDetailModel()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var entity = new PlaylistEntity
            {
                Id = Guid.NewGuid(),
                Name = "Playlist 1",
                Description = "Description 1",
                CoverImage = "cover.jpg",
                Favorite = false,
                CreatedAt = now.AddHours(-1),
                UpdatedAt = now
            };

            // Act
            var detailModel = _mapper.MapToDetailModel(entity);

            // Assert
            Assert.Equal(entity.Id, detailModel.Id);
            Assert.Equal(entity.Name, detailModel.Name);
            Assert.Equal(entity.Description, detailModel.Description);
            Assert.Equal(entity.CoverImage, detailModel.CoverImage);
            Assert.Equal(entity.Favorite, detailModel.Favorite);
            Assert.Equal(entity.CreatedAt, detailModel.CreatedAt);
            Assert.Equal(entity.UpdatedAt, detailModel.UpdatedAt);
        }

        [Fact]
        public void MapToEntity_WithValidDetailModel_ReturnsPlaylistEntity()
        {
            // Arrange
            var detailModel = new PlaylistDetailModel
            {
                Id = Guid.NewGuid(),
                Name = "Playlist 1",
                Description = "Description 1",
                CoverImage = "cover.jpg",
                Favorite = true,
                CreatedAt = DateTime.UtcNow.AddHours(-1),
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            PlaylistEntity entity = _mapper.MapToEntity(detailModel);

            // Assert
            Assert.Equal(detailModel.Id, entity.Id);
            Assert.Equal(detailModel.Name, entity.Name);
            Assert.Equal(detailModel.Description, entity.Description);
            Assert.Equal(detailModel.CoverImage, entity.CoverImage);
            Assert.Equal(detailModel.Favorite, entity.Favorite);
            Assert.Equal(detailModel.CreatedAt, entity.CreatedAt);
            Assert.Equal(detailModel.UpdatedAt, entity.UpdatedAt);
        }
    }
}

