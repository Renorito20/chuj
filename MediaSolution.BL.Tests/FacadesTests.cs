using Xunit;
using MediaSolution.BL.Facades;
using MediaSolution.BL.Mappers;
using MediaSolution.DAL.UnitOfWork;
using MediaSolution.DAL.Factories;



namespace MediaSolution.BL.Tests.Facades
{
    public class FacadesTests
    {
        [Fact]
        public void MediaFacade_CanBeInstantiated()
        {
            // Arrange & Act
            var dbContextFactory = new DbContextSQLiteFactory("test.db");
            var unitOfWorkFactory = new UnitOfWorkFactory(dbContextFactory); 
            var modelMapper = new MediaModelMapper();
            var mediaFacade = new MediaFacade(unitOfWorkFactory, modelMapper);

            // Assert
            Assert.NotNull(mediaFacade);
        }

        [Fact]
        public void PlaylistFacade_CanBeInstantiated()
        {
            // Arrange & Act
            var dbContextFactory = new DbContextSQLiteFactory("test.db");
            var unitOfWorkFactory = new UnitOfWorkFactory(dbContextFactory);
            var MmodelMapper = new MediaModelMapper();
            var modelMapper = new PlaylistModelMapper(MmodelMapper);
            var playlistFacade = new PlaylistFacade(unitOfWorkFactory, modelMapper);

            // Assert
            Assert.NotNull(playlistFacade);
        }

        [Fact]
        public void PlaylistMediaFacade_CanBeInstantiated()
        {
            // Arrange & Act
            var dbContextFactory = new DbContextSQLiteFactory("test.db");
            var unitOfWorkFactory = new UnitOfWorkFactory(dbContextFactory);
            var modelMapper = new PlaylistMediaModelMapper();
            var playlistMediaFacade = new PlaylistMediaFacade(unitOfWorkFactory,modelMapper);

            // Assert
            Assert.NotNull(playlistMediaFacade);
        }
    }
}