using iTaaS.ConvertLogService.Models;
using System;
using System.Collections.Generic;

namespace iTaaS.ConvertLogService.Services.Interfaces
{
    public interface ISourceService
    {
        string SaveSource(Source source);
        IEnumerable<Source> ListAllSources();
        Source GetSourceById(Guid id);
    }
}
