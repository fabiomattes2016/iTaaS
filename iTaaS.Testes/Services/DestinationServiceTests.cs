using iTaaS.ConvertLogService.Models;
using iTaaS.ConvertLogService.Repositories.Interfaces;
using iTaaS.ConvertLogService.Services;
using iTaaS.ConvertLogService.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace iTaaS.Testes.Services
{
    public class DestinationServiceTests
    {
        private readonly Mock<ISourceRepository> _repositoryMock;
        private readonly Mock<IHttpClientWrapper> _httpClientMock;
        private readonly SourceService _service;

        public DestinationServiceTests()
        {
            _repositoryMock = new Mock<ISourceRepository>();
            _httpClientMock = new Mock<IHttpClientWrapper>();
            _service = new SourceService(_repositoryMock.Object, _httpClientMock.Object);
        }

        [Fact]
        public void GetSourceById_ReturnsCorrectSource()
        {
            var sourceId = Guid.NewGuid();
            var expectedSource = new Source { Id = sourceId, Url = "http://example.com" };

            _repositoryMock.Setup(repo => repo.GetSourceById(sourceId)).Returns(expectedSource);

            var result = _service.GetSourceById(sourceId);

            Assert.NotNull(result);
            Assert.Equal(expectedSource.Id, result.Id);
            Assert.Equal(expectedSource.Url, result.Url);
        }

        [Fact]
        public void ListAllSources_ReturnsAllSources()
        {
            var sources = new List<Source>
            {
                new Source { Id = Guid.NewGuid(), Url = "http://example.com/1" },
                new Source { Id = Guid.NewGuid(), Url = "http://example.com/2" }
            };

            _repositoryMock.Setup(repo => repo.ListAllSources()).Returns(sources);

            var results = _service.ListAllSources();

            Assert.NotNull(results);
            Assert.Equal(sources.Count, results.Count());
        }

        [Fact]
        public void SaveSource_SavesAndReturnsContent()
        {
            var source = new Source { Url = "http://example.com" };
            var content = "Sample content";

            _httpClientMock.Setup(client => client.GetStringAsync(source.Url)).ReturnsAsync(content);

            var result = _service.SaveSource(source);

            _repositoryMock.Verify(repo => repo.SaveSource(It.Is<Source>(s => s.Url == source.Url && s.Log == content)), Times.Once);
            Assert.Equal(content, result);
        }

    }
}
