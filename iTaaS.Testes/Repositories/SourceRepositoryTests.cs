using iTaaS.ConvertLogService.Models;
using iTaaS.ConvertLogService.Repositories;
using iTaaS.ConvertLogService.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace iTaaS.ConvertLogService.Tests
{
    public class SourceRepositoryTests
    {
        private DbContextOptions<AppDbContext> CreateInMemoryDatabase()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")  // Nome arbitrário para o banco em memória
                .Options;
        }

        [Fact]
        public void ListAllSources_MustBeReturnListOfSources()
        {
            // Arrange
            var options = CreateInMemoryDatabase();
            string log = "312|200|HIT| GET / robots.txt HTTP / 1.1 |100.2 " +
                "101 | 200 | MISS | POST /myImages HTTP/1.1 | 319.4 " +
                "199 | 404 | MISS | GET /not-found HTTP/1.1 | 142.9 " +
                "312 | 200 | INVALIDATE | GET /robots.txt HTTP/1.1 | 245.1";


            var mockData = new List<Source>
            {
                new Source { Id = Guid.NewGuid(), Url = "http://www.example.com", Log = log },
                new Source { Id = Guid.NewGuid(), Url = "http://www.example.com", Log = log },
                new Source { Id = Guid.NewGuid(), Url = "http://www.example.com", Log = log },
            };

            // Usando InMemoryDatabase para simular o contexto
            using (var context = new AppDbContext(options))
            {
                context.Sources.AddRange(mockData);
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new SourceRepository(context);

                // Act
                var result = repository.ListAllSources();

                // Assert
                var resultList = result.ToList();
                Assert.Equal(5, resultList.Count);
                Assert.Equal(log, resultList[0].Log);
                Assert.Equal(log, resultList[1].Log);
            }
        }

        [Fact]
        public void SaveSource_MustSaveSource_WhenSourceIsValid()
        {
            // Arrange
            var options = CreateInMemoryDatabase();
            string log = "312|200|HIT| GET / robots.txt HTTP / 1.1 |100.2 " +
                "101 | 200 | MISS | POST /myImages HTTP/1.1 | 319.4 " +
                "199 | 404 | MISS | GET /not-found HTTP/1.1 | 142.9 " +
                "312 | 200 | INVALIDATE | GET /robots.txt HTTP/1.1 | 245.1";

            var source = new Source { Id = Guid.NewGuid(), Url = "http://example.com", Log = log };

            using (var context = new AppDbContext(options))
            {
                var repository = new SourceRepository(context);

                // Act
                repository.SaveSource(source);

                // Assert
                var savedSource = context.Sources.FirstOrDefault(s => s.Id == source.Id);
                Assert.NotNull(savedSource);  // Verifica se o Source foi salvo
                Assert.Equal("http://example.com", savedSource.Url);  // Verifica se a URL foi salva corretamente
                Assert.Equal(log, savedSource.Log);  // Verifica se o Log foi salvo corretamente
            }
        }

        [Fact]
        public void SaveSource_ShouldThrowArgumentNullException_WhenSourceIsNull()
        {
            // Arrange
            var options = CreateInMemoryDatabase();
            Source source = null;

            using (var context = new AppDbContext(options))
            {
                var repository = new SourceRepository(context);

                // Act & Assert
                var exception = Assert.Throws<ArgumentNullException>(() => repository.SaveSource(source));
                Assert.Equal("source", exception.ParamName);  // Verifica se a exceção é lançada corretamente
            }
        }

        [Fact]
        public void SaveSource_ShouldThrowArgumentException_WhenUrlIsNullOrEmpty()
        {
            // Arrange
            var options = CreateInMemoryDatabase();
            string log = "312|200|HIT| GET / robots.txt HTTP / 1.1 |100.2 " +
                "101 | 200 | MISS | POST /myImages HTTP/1.1 | 319.4 " +
                "199 | 404 | MISS | GET /not-found HTTP/1.1 | 142.9 " +
                "312 | 200 | INVALIDATE | GET /robots.txt HTTP/1.1 | 245.1";

            var invalidSource = new Source { Id = Guid.NewGuid(), Url = null, Log = log };  // Url é null

            using (var context = new AppDbContext(options))
            {
                var repository = new SourceRepository(context);

                // Act & Assert
                var exception = Assert.Throws<ArgumentException>(() => repository.SaveSource(invalidSource));
                Assert.Equal("URL is required\r\nParameter name: Url", exception.Message);  // Verifica a mensagem da exceção
                Assert.Equal("Url", exception.ParamName);  // Verifica o nome do parâmetro que causou a exceção
            }
        }

        [Fact]
        public void GetSourceById_MustReturnSource_WhenIdExists()
        {
            // Arrange
            var options = CreateInMemoryDatabase();
            string log = "312|200|HIT| GET / robots.txt HTTP / 1.1 |100.2 " +
                "101 | 200 | MISS | POST /myImages HTTP/1.1 | 319.4 " +
                "199 | 404 | MISS | GET /not-found HTTP/1.1 | 142.9 " +
                "312 | 200 | INVALIDATE | GET /robots.txt HTTP/1.1 | 245.1";

            var expectedSource = new Source { Id = Guid.NewGuid(), Url = "http://example.com", Log = log };

            using (var context = new AppDbContext(options))
            {
                context.Sources.Add(expectedSource);
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new SourceRepository(context);

                // Act
                var result = repository.GetSourceById(expectedSource.Id);

                // Assert
                Assert.NotNull(result);  // Verifica se o resultado não é nulo
                Assert.Equal(expectedSource.Id, result.Id);  // Verifica se o ID do Source é o mesmo
                Assert.Equal(expectedSource.Url, result.Url);  // Verifica a URL
                Assert.Equal(expectedSource.Log, result.Log);  // Verifica o Log
            }
        }
    }
}
