using iTaaS.ConvertLogService.Models;
using System;
using System.Collections.Generic;

namespace iTaaS.ConvertLogService.Services.Interfaces
{
    public interface IDestinationService
    {
        void SaveTransformedLog(Destination destination);
        IEnumerable<Destination> GetDestinations();
        Destination GetDestination(Guid id);
    }
}
