using iTaaS.ConvertLogService.Models;
using iTaaS.ConvertLogService.Repositories.Interfaces;
using iTaaS.ConvertLogService.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace iTaaS.ConvertLogService.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository _repository;

        public DestinationService(IDestinationRepository repository)
        {
            _repository = repository;
        }

        public Destination GetDestination(Guid id)
        {
            return _repository.GetDestination(id);
        }

        public IEnumerable<Destination> GetDestinations()
        {
            return _repository.GetDestinations();
        }

        public void SaveTransformedLog(Destination destination)
        {
            _repository.SaveTransformedLog(destination);
        }
    }
}
