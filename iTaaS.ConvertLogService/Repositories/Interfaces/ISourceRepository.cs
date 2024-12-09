using iTaaS.ConvertLogService.Models;
using System;
using System.Collections.Generic;

namespace iTaaS.ConvertLogService.Repositories.Interfaces
{
    public interface ISourceRepository
    {
        Guid SaveSource(Source source);
        IEnumerable<Source> ListAllSources();
        Source GetSourceById(Guid id);
    }
}
