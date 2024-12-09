using iTaaS.ConvertLogService.Models;
using System;
using System.Collections.Generic;

namespace iTaaS.ConvertLogService.Repositories.Interfaces
{
    public interface IDestinationRepository
    {
        void SaveTransformedLog(Destination destination);
        IEnumerable<Destination> GetDestinations();
        Destination GetDestination(Guid id);
    }
}
