using MediaSolution.DAL.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaSolution.DAL.Seeds;

    public class DbSeeder(IDbContextFactory<MediaSolutionDbContext> dbContextFactory, IOptions<DALOptions> options)
        : IDbSeeder
    {
        public void SeedDatabase()
        {
            using MediaSolutionDbContext dbContext = dbContextFactory.CreateDbContext();

            if (options.Value.SeedDemoData)
            {
                
                dbContext
                    .SeedMedia()
                    .SeedPlaylists()
                    .SeedPlaylistMedia();
               
                dbContext.SaveChanges();
            }
        }
    }

