using AutoMapper;
using iTaaS.ConvertLogService.Controllers;
using iTaaS.ConvertLogService.DTOs;
using iTaaS.ConvertLogService.Models;
using iTaaS.ConvertLogService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace iTaaS.Testes.Controllers
{
    public class SourceControllerTests
    {
        private readonly Mock<ISourceService> _serviceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SourceController _controller;

        public SourceControllerTests()
        {
            _serviceMock = new Mock<ISourceService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new SourceController(_mapperMock.Object, _serviceMock.Object);
        }

        [Fact]
        public void GetSources_ShouldReturnOkResult_WithMappedDTOs()
        {
            // Arrange
            var sources = new List<Source>
            {
                new Source { Id = Guid.NewGuid(), Url = "http://example1.com", Log = "Log1" },
                new Source { Id = Guid.NewGuid(), Url = "http://example2.com", Log = "Log2" }
            };
            var sourceDtos = new List<SourceReadDTO>
            {
                new SourceReadDTO { Id = sources[0].Id, Url = sources[0].Url, Log = sources[0].Log, Created = DateTime.UtcNow },
                new SourceReadDTO { Id = sources[1].Id, Url = sources[1].Url, Log = sources[1].Log, Created = DateTime.UtcNow }
            };
            _serviceMock.Setup(s => s.ListAllSources()).Returns(sources);
            _mapperMock.Setup(m => m.Map<IEnumerable<SourceReadDTO>>(It.IsAny<IEnumerable<Source>>())).Returns(sourceDtos);

            // Act
            var result = _controller.GetSources();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<SourceReadDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<SourceReadDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public void GetSourceById_ShouldReturnOkResult_WhenSourceExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var source = new Source { Id = id, Url = "http://example.com", Log = "Log content" };
            var sourceDto = new SourceReadDTO { Id = id, Url = "http://example.com", Log = "Log content", Created = DateTime.UtcNow };

            _serviceMock.Setup(s => s.GetSourceById(id)).Returns(source);
            _mapperMock.Setup(m => m.Map<SourceReadDTO>(It.IsAny<Source>())).Returns(sourceDto);

            // Act
            var result = _controller.GetSourceById(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SourceReadDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<SourceReadDTO>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public void GetSourceById_ShouldReturnNotFound_WhenSourceDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.GetSourceById(id)).Returns((Source)null);

            // Act
            var result = _controller.GetSourceById(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SourceReadDTO>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public void CreateSource_ShouldReturnOkResult_WithCreatedSource()
        {
            // Arrange
            var sourceCreateDto = new SourceCreateDTO { Url = "http://newsource.com" };
            var source = new Source { Id = Guid.NewGuid(), Url = "http://newsource.com", Log = "Some content" };
            var sourceDto = new SourceReadDTO { Url = source.Url, Log = source.Log, Created = DateTime.UtcNow };
            var content = "Some content from URL";

            _mapperMock.Setup(m => m.Map<Source>(It.IsAny<SourceCreateDTO>())).Returns(source);
            _serviceMock.Setup(s => s.SaveSource(source)).Returns(content);
            _mapperMock.Setup(m => m.Map<SourceReadDTO>(It.IsAny<Source>())).Returns(sourceDto);

            // Act
            var result = _controller.CreateSource(sourceCreateDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SourceReadDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            // Cast do valor retornado para o tipo anônimo
            var returnValue = okResult.Value as dynamic;

            // Verifique as propriedades usando 'dynamic'
            Assert.Equal(content, returnValue.Log);
            Assert.Equal(source.Url, returnValue.Url);
            Assert.Equal(sourceDto.Created, returnValue.Created);
        }

    }
}
