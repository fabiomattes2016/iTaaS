using AutoMapper;
using iTaaS.ConvertLogService.DTOs;
using iTaaS.ConvertLogService.Models;
using iTaaS.ConvertLogService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace iTaaS.ConvertLogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISourceService _service;

        public SourceController(IMapper mapper, ISourceService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SourceReadDTO>> GetSources()
        {
            var sources = _service.ListAllSources();

            return Ok(_mapper.Map<IEnumerable<SourceReadDTO>>(sources));
        }

        [HttpGet("{id}", Name = "GetSourceById")]
        public ActionResult<SourceReadDTO> GetSourceById(Guid id)
        {
            var source = _service.GetSourceById(id);

            if (source == null)
            {
                return NotFound(new {
                    Status = 404,
                    Message = "Source not found"
                });
            }

            return Ok(_mapper.Map<SourceReadDTO>(source));
        }

        [HttpPost]
        public ActionResult<SourceReadDTO> CreateSource(SourceCreateDTO sourceCreateDto)
        {
            var source = _mapper.Map<Source>(sourceCreateDto);

            var content = _service.SaveSource(source);

            var sourceReadDto = _mapper.Map<SourceReadDTO>(source);

            return Ok(new SourceReadDTO
            {
                Url = sourceReadDto.Url,
                Log = content,
                Created = sourceReadDto.Created,
            });
        }
    }
}
