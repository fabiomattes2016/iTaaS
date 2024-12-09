using iTaaS.ConvertLogService.Models;
using iTaaS.ConvertLogService.Repositories.Interfaces;
using iTaaS.ConvertLogService.Services;
using iTaaS.ConvertLogService.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;

namespace iTaaS.Testes.Services
{
    public class SourceServiceTests
    {
        private readonly Mock<ISourceRepository> _repositoryMock;
        private readonly SourceService _service;
        private readonly IHttpClientWrapper _httpClientWrapper;

        public SourceServiceTests()
        {
            _repositoryMock = new Mock<ISourceRepository>();
            _service = new SourceService(_repositoryMock.Object, _httpClientWrapper);
        }

        [Fact]
        public void GetSourceById_MustReturnSource_WhenIdExists()
        {
            string log = "312|200|HIT| GET / robots.txt HTTP / 1.1 |100.2 " +
                "101 | 200 | MISS | POST /myImages HTTP/1.1 | 319.4 " +
                "199 | 404 | MISS | GET /not-found HTTP/1.1 | 142.9 " +
                "312 | 200 | INVALIDATE | GET /robots.txt HTTP/1.1 | 245.1";

            // Arrange
            var expectedSource = new Source { Id = Guid.NewGuid(), Url = "http://example.com", Log = log };
            _repositoryMock.Setup(r => r.GetSourceById(It.IsAny<Guid>())).Returns(expectedSource);

            // Act
            var result = _service.GetSourceById(expectedSource.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSource.Id, result.Id);
            Assert.Equal(expectedSource.Url, result.Url);
            Assert.Equal(expectedSource.Log, result.Log);
        }

        [Fact]
        public void ListAllSources_MustReturnListOfSources()
        {
            string log = "312|200|HIT| GET / robots.txt HTTP / 1.1 |100.2 " +
                "101 | 200 | MISS | POST /myImages HTTP/1.1 | 319.4 " +
                "199 | 404 | MISS | GET /not-found HTTP/1.1 | 142.9 " +
                "312 | 200 | INVALIDATE | GET /robots.txt HTTP/1.1 | 245.1";

            // Arrange
            var sourceList = new List<Source>
            {
                new Source { Id = Guid.NewGuid(), Url = "http://example1.com", Log = log },
                new Source { Id = Guid.NewGuid(), Url = "http://example2.com", Log = log }
            };

            _repositoryMock.Setup(r => r.ListAllSources()).Returns(sourceList);

            // Act
            var result = _service.ListAllSources();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(sourceList[0].Log, result.First().Log);
            Assert.Equal(sourceList[1].Log, result.Last().Log);
        }

        [Fact]
        public void SaveSource_MustSaveSource_WhenUrlIsValid()
        {
            // Arrange
            var source = new Source { Url = "http://example.com" };
            var expectedContent = "Some content read from the URL";

            // Mock do IHttpClientWrapper
            var httpClientMock = new Mock<IHttpClientWrapper>();
            httpClientMock.Setup(c => c.GetStringAsync(It.IsAny<string>())).ReturnsAsync(expectedContent);

            // Mock do repositório
            _repositoryMock.Setup(r => r.SaveSource(It.IsAny<Source>())).Verifiable();

            var service = new SourceService(_repositoryMock.Object, httpClientMock.Object);

            // Act
            var result = service.SaveSource(source);

            // Assert
            Assert.Equal(expectedContent, result);
            _repositoryMock.Verify(r => r.SaveSource(It.Is<Source>(s => s.Log == expectedContent && s.Url == source.Url)), Times.Once);
        }
    }
}
