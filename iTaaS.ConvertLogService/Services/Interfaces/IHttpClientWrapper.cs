using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iTaaS.ConvertLogService.Services.Interfaces
{
    public interface IHttpClientWrapper
    {
        Task<string> GetStringAsync(string url);
    }
}
