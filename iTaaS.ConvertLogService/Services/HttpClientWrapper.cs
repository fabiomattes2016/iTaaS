using iTaaS.ConvertLogService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace iTaaS.ConvertLogService.Services
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper()
        {
            _httpClient = new HttpClient();
        }

        public Task<string> GetStringAsync(string url)
        {
            return _httpClient.GetStringAsync(url);
        }
    }
}
