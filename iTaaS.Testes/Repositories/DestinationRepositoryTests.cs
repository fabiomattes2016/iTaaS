using iTaaS.ConvertLogService.Data;
using iTaaS.ConvertLogService.Models;
using iTaaS.ConvertLogService.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace iTaaS.Testes.Repositories
{
    public class DestinationRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public DestinationRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            using (var context = new AppDbContext(_options))
            {
                context.Destinations.AddRange(new List<Destination>
                {
                    new Destination { Id = Guid.NewGuid(), Name = "Destination 1" },
                    new Destination { Id = Guid.NewGuid(), Name = "Destination 2" }
                });
                context.SaveChanges();
            }
        }

        [Fact]
        public void GetDestination_ReturnsCorrectDestination()
        {
            using (var context = new AppDbContext(_options))
            {
                var repository = new DestinationRepository(context);
                var destination = context.Destinations.First();
                var result = repository.GetDestination(destination.Id);

                Assert.NotNull(result);
                Assert.Equal(destination.Id, result.Id);
            }
        }

        [Fact]
        public void GetDestinations_ReturnsAllDestinations()
        {
            using (var context = new AppDbContext(_options))
            {
                var repository = new DestinationRepository(context);
                var results = repository.GetDestinations();

                Assert.NotNull(results);
                Assert.Equal(3, results.Count());
            }
        }

        [Fact]
        public void SaveTransformedLog_AddsDestination()
        {
            using (var context = new AppDbContext(_options))
            {
                var repository = new DestinationRepository(context);
                var newDestination = new Destination { Id = Guid.NewGuid(), Name = "Destination 3" };

                repository.SaveTransformedLog(newDestination);
                var result = context.Destinations.FirstOrDefault(d => d.Id == newDestination.Id);

                Assert.NotNull(result);
                Assert.Equal(newDestination.Name, result.Name);
            }
        }
    }
}
