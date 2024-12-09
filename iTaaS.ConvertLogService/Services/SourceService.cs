using iTaaS.ConvertLogService.Models;
using iTaaS.ConvertLogService.Repositories.Interfaces;
using iTaaS.ConvertLogService.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace iTaaS.ConvertLogService.Services
{
    public class SourceService : ISourceService
    {
        private readonly ISourceRepository _repository;
        private readonly IHttpClientWrapper _httpClient;

        public SourceService(ISourceRepository repository, IHttpClientWrapper httpClient)
        {
            _repository = repository;
            _httpClient = httpClient;
        }

        public Source GetSourceById(Guid id)
        {
            return _repository.GetSourceById(id);
        }

        public IEnumerable<Source> ListAllSources()
        {
            return _repository.ListAllSources();
        }

        public string SaveSource(Source source)
        {
            string url = source.Url;
            string conteudoLido;

            try
            {
                conteudoLido = _httpClient.GetStringAsync(url).Result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            var newSource = new Source
            {
                Log = conteudoLido,
                Url = url
            };

            _repository.SaveSource(newSource);

            return conteudoLido;
        }
    }
}
