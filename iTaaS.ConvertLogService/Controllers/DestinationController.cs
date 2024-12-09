using AutoMapper;
using iTaaS.ConvertLogService.DTOs;
using iTaaS.ConvertLogService.Models;
using iTaaS.ConvertLogService.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace iTaaS.ConvertLogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDestinationService _service;
        private readonly ISourceService _sourceService;
        private readonly IHostingEnvironment _env;
        private readonly IHttpClientWrapper _httpClient;

        public DestinationController(
            IMapper mapper, 
            IDestinationService service, 
            ISourceService sourceService,
            IHostingEnvironment env,
            IHttpClientWrapper httpClient
        )
        {
            _mapper = mapper;
            _service = service;
            _sourceService = sourceService;
            _env = env;
            _httpClient = httpClient;
        }

        [HttpPost("convert")]
        public ActionResult<DestinationReadDTO> CreateDestination(DestinationCreateDTO destinationCreateDTO, [FromQuery] Guid id, [FromQuery] string response = "response")
        {
            Source source;            
            Destination destination = new Destination();
            DestinationReadDTO destinationReadDTO = new DestinationReadDTO();
            string converted;

            if (id != null && destinationCreateDTO.Url == "" || destinationCreateDTO.Url == null)
            {
                source = _sourceService.GetSourceById(id);

                if (source == null)
                {
                    return NotFound(new
                    {
                        Status = 404,
                        Message = "Source not found"
                    });
                }

                converted = Convert(source.Log);

                destination.OriginalLog = source.Log;
                destination.ConvertedLog = converted;
            }
            else
            {
                string conteudoLido = _httpClient.GetStringAsync(destinationCreateDTO.Url).Result;

                Source newSource = new Source
                { 
                    Url = destinationCreateDTO.Url,
                    Log = conteudoLido
                };

                _sourceService.SaveSource(newSource);
                
                converted = Convert(conteudoLido);

                destination.Url = destinationCreateDTO.Url;
                destination.OriginalLog = conteudoLido;
                destination.ConvertedLog = converted;
            }

            if (response == "response")
            {
                destinationReadDTO = SettingMap(destination);

                destinationReadDTO.OriginalLog = destination.OriginalLog;
                destinationReadDTO.Url = destination.Url;
            }
            else
            {
                destinationReadDTO = SettingMap(destination);

                string dataHoraArquivo = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string guid = Guid.NewGuid().ToString();
                string nomeArquivoNovo = $"{dataHoraArquivo}_{guid}.txt";

                string[] linhasLidas = destinationReadDTO.ConvertedLog.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                destinationReadDTO.FilePath = SaveLog(linhasLidas, nomeArquivoNovo);
                destinationReadDTO.OriginalLog = destination.OriginalLog;
                destinationReadDTO.Url = destination.Url;
            }

            var newDestination = _mapper.Map<Destination>(destinationReadDTO);
            _service.SaveTransformedLog(newDestination);

            return Ok(new DestinationReadDTO
            { 
                Url = destinationReadDTO.Url,
                FilePath = destinationReadDTO.FilePath,
                ConvertedLog = destinationReadDTO.ConvertedLog,
                OriginalLog = destinationReadDTO.OriginalLog,
                Created = destinationReadDTO.Created
            });
        }

        [HttpGet]
        public ActionResult<IEnumerable<DestinationReadDTO>> ListAllDestinations()
        {
            var destinations = _service.GetDestinations();

            return Ok(_mapper.Map<IEnumerable<DestinationReadDTO>>(destinations));
        }

        [HttpGet("{id}")]
        public ActionResult<DestinationReadDTO> GetDestinationById(Guid id)
        {
            var destination = _service.GetDestination(id);

            if (destination == null)
            {
                return NotFound(new
                {
                    Status = 404,
                    Message = "Destination not found"
                });
            }

            return Ok(_mapper.Map<DestinationReadDTO>(destination));
        }

        private DestinationReadDTO SettingMap(Destination destination)
        {
            DestinationReadDTO destinationReadDTO = new DestinationReadDTO();

            var destinationDTO = _mapper.Map<DestinationReadDTO>(destination);

            destinationReadDTO.Id = destinationDTO.Id;
            destinationReadDTO.ConvertedLog = destinationDTO.ConvertedLog;
            destinationReadDTO.Created = destinationDTO.Created;

            return destinationReadDTO;
        }

        private string Convert(string input)
        {
            var lines = input.Split('\n');

            var transformedLog = new List<string>
            {
                "#Version: 1.0",
                $"#Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}",
                "#Fields: provider http-method status-code uri-path time-taken response-size cache-status"
            };

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('|');
                var responseSize = (int)double.Parse(parts[4]);
                var cacheStatus = parts[2] == "INVALIDATE" ? "REFRESH_HIT" : parts[2];

                transformedLog.Add($"\"MINHA CDN\" {parts[3].Split(' ')[0]} {parts[1]} {parts[3].Split(' ')[1]} {responseSize} {parts[0]} {cacheStatus}");
            }

            return string.Join("\n", transformedLog);
        }

        private string SaveLog(string[] content, string filename)
        {
            // Diretório "wwwroot/downloads"
            var filePath = Path.Combine(_env.WebRootPath, "logs", filename);

            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            System.IO.File.WriteAllLines(filePath, content);

            return $"/logs/{filename}"; // URL para acessar o arquivo
        }
    }
}
