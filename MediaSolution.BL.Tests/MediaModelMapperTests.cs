using System;
using MediaSolution.BL.Mappers;
using MediaSolution.BL.Models;
using MediaSolution.DAL.Entities;
using Xunit;

namespace MediaSolution.BL.Tests.Mappers
{
    public class MediaModelMapperTests
    {
        private readonly MediaModelMapper _mapper = new MediaModelMapper();

        [Fact]
        public void MapToListModel_WithValidEntity_ReturnsMediaListModel()
        {
            // Arrange
            var entity = new MediaEntity
            {
                Id = Guid.NewGuid(),
                Name = "Sample Media",
                Duration = TimeSpan.FromMinutes(3),
                SizeInBytes = 1024,
                Authors = "Author A",
                Path = "/sample/path/A"
            };

            // Act
            var listModel = _mapper.MapToListModel(entity);

            // Assert
            Assert.Equal(entity.Id, listModel.Id);
            Assert.Equal(entity.Name, listModel.Name);
            Assert.Equal(entity.Duration, listModel.Duration);
            Assert.Equal(entity.Authors, listModel.Authors);
        }

        [Fact]
        public void MapToDetailModel_WithValidEntity_ReturnsMediaDetailModel()
        {
            // Arrange
            var entity = new MediaEntity
            {
                Id = Guid.NewGuid(),
                Name = "Sample Media",
                Duration = TimeSpan.FromMinutes(3),
                SizeInBytes = 1024,
                CreatedAt = DateTime.UtcNow.AddHours(-1),
                UpdatedAt = DateTime.UtcNow,
                Authors = "Author A",
                Genres = "Genre A",
                Path = "/sample/path/A"
            };

            // Act
            var detailModel = _mapper.MapToDetailModel(entity);

            // Assert
            Assert.Equal(entity.Id, detailModel.Id);
            Assert.Equal(entity.Name, detailModel.Name);
            Assert.Equal(entity.Duration, detailModel.Duration);
            Assert.Equal(entity.SizeInBytes, detailModel.SizeInBytes);
            Assert.Equal(entity.Authors, detailModel.Authors);
            Assert.Equal(entity.Genres, detailModel.Genres);
            Assert.Equal(entity.Path, detailModel.Path);
        }

        [Fact]
        public void MapToEntity_WithValidDetailModel_ReturnsMediaEntity()
        {
            // Arrange
            var detailModel = new MediaDetailModel
            {
                Id = Guid.NewGuid(),
                Name = "Sample Media",
                Duration = TimeSpan.FromMinutes(3),
                SizeInBytes = 1024,
                CreatedAt = DateTime.UtcNow.AddHours(-1),
                UpdatedAt = DateTime.UtcNow,
                Authors = "Author A",
                Genres = "Genre A",
                Path = "/sample/path"
            };

            // Act
            MediaEntity entity = _mapper.MapToEntity(detailModel);

            // Assert
            Assert.Equal(detailModel.Id, entity.Id);
            Assert.Equal(detailModel.Name, entity.Name);
            Assert.Equal(detailModel.Duration, entity.Duration);
            Assert.Equal(detailModel.SizeInBytes, entity.SizeInBytes);
            Assert.Equal(detailModel.Authors, entity.Authors);
            Assert.Equal(detailModel.Genres, entity.Genres);
            Assert.Equal(detailModel.Path, entity.Path);
        }
    }
}
