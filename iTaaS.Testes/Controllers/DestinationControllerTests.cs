using AutoMapper;
using iTaaS.ConvertLogService.Controllers;
using iTaaS.ConvertLogService.Data;
using iTaaS.ConvertLogService.DTOs;
using iTaaS.ConvertLogService.Models;
using iTaaS.ConvertLogService.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace iTaaS.Testes.Controllers
{
    public class DestinationControllerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ISourceService> _sourceServiceMock;
        private readonly Mock<IHostingEnvironment> _envMock;
        private readonly Mock<IHttpClientWrapper> _httpClientMock;
        private readonly DestinationController _controller;
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly AppDbContext _context;

        public DestinationControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _sourceServiceMock = new Mock<ISourceService>();
            _envMock = new Mock<IHostingEnvironment>();
            _httpClientMock = new Mock<IHttpClientWrapper>();

            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            _context = new AppDbContext(_options);

            var destinationServiceMock = new Mock<IDestinationService>();
            destinationServiceMock.Setup(service => service.GetDestinations())
                .Returns(() => _context.Destinations.ToList());

            destinationServiceMock.Setup(service => service.GetDestination(It.IsAny<Guid>()))
                .Returns((Guid id) => _context.Destinations.FirstOrDefault(d => d.Id == id));

            destinationServiceMock.Setup(service => service.SaveTransformedLog(It.IsAny<Destination>()))
                .Callback((Destination destination) =>
                {
                    _context.Destinations.Add(destination);
                    _context.SaveChanges();
                });

            _controller = new DestinationController(
                _mapperMock.Object,
                destinationServiceMock.Object,
                _sourceServiceMock.Object,
                _envMock.Object,
                _httpClientMock.Object
            );
        }

        [Fact]
        public void CreateDestination_ReturnsOk_WithDestinationReadDTO()
        {
            // Arrange
            var destinationCreateDTO = new DestinationCreateDTO { Url = "http://example.com" };
            var sourceId = Guid.NewGuid();
            var source = new Source { Id = sourceId, Log = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2" };
            var destinationReadDTO = new DestinationReadDTO
            {
                Id = Guid.NewGuid(),
                Url = "http://example.com",
                OriginalLog = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2",
                ConvertedLog = "#Version: 1.0\n#Date: {Date}\n#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT",
                Created = DateTime.UtcNow
            };

            _sourceServiceMock.Setup(service => service.GetSourceById(sourceId)).Returns(source);
            _httpClientMock.Setup(client => client.GetStringAsync(destinationCreateDTO.Url)).ReturnsAsync("312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2");
            _sourceServiceMock.Setup(service => service.SaveSource(It.IsAny<Source>()));
            _mapperMock.Setup(mapper => mapper.Map<DestinationReadDTO>(It.IsAny<Destination>())).Returns(destinationReadDTO);
            _mapperMock.Setup(mapper => mapper.Map<Destination>(It.IsAny<DestinationReadDTO>())).Returns(new Destination
            {
                Id = destinationReadDTO.Id,
                Url = destinationReadDTO.Url,
                FilePath = destinationReadDTO.FilePath,
                ConvertedLog = destinationReadDTO.ConvertedLog,
                OriginalLog = destinationReadDTO.OriginalLog,
                Created = destinationReadDTO.Created
            });

            // Act
            var result = _controller.CreateDestination(destinationCreateDTO, sourceId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedValue = Assert.IsType<DestinationReadDTO>(actionResult.Value);
            Assert.Equal("http://example.com", returnedValue.Url);
            Assert.Equal("#Version: 1.0\n#Date: {Date}\n#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT", returnedValue.ConvertedLog);
        }


        [Fact]
        public void ListAllDestinations_ReturnsOk_WithListOfDestinations()
        {
            // Arrange
            var destinations = new List<Destination>
            {
                new Destination { Id = Guid.NewGuid(), OriginalLog = "Log 1", ConvertedLog = "Converted log 1" },
                new Destination { Id = Guid.NewGuid(), OriginalLog = "Log 2", ConvertedLog = "Converted log 2" }
            };

            _context.Destinations.AddRange(destinations);
            _context.SaveChanges();

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<DestinationReadDTO>>(It.IsAny<IEnumerable<Destination>>()))
                .Returns((IEnumerable<Destination> src) =>
                {
                    return src.Select(d => new DestinationReadDTO { Id = d.Id, ConvertedLog = d.ConvertedLog, Url = d.Url, OriginalLog = d.OriginalLog, FilePath = d.FilePath, Created = d.Created }).ToList();
                });

            // Act
            var result = _controller.ListAllDestinations();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsType<List<DestinationReadDTO>>(actionResult.Value);
            Assert.Equal(11, returnedList.Count);
        }

        [Fact]
        public void GetDestinationById_ReturnsOk_WithDestinationReadDTO()
        {
            // Arrange
            var destinationId = Guid.NewGuid();
            var destination = new Destination { Id = destinationId, OriginalLog = "Log content", ConvertedLog = "Converted log" };
            var destinationReadDTO = new DestinationReadDTO { Id = destinationId, ConvertedLog = "Converted log", OriginalLog = "Log content", Url = "http://example.com" };

            _context.Destinations.Add(destination);
            _context.SaveChanges();

            _mapperMock.Setup(mapper => mapper.Map<DestinationReadDTO>(destination)).Returns(destinationReadDTO);

            // Act
            var result = _controller.GetDestinationById(destinationId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedValue = Assert.IsType<DestinationReadDTO>(actionResult.Value);
            Assert.Equal("Converted log", returnedValue.ConvertedLog);
            Assert.Equal("Log content", returnedValue.OriginalLog);
            Assert.Equal("http://example.com", returnedValue.Url);
        }
    }
}
