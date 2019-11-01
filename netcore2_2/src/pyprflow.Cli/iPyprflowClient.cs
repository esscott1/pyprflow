using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace pyprflow.Cli
{
    public class iPyprflowClient
    {
        private readonly HttpClient _httpClient;
        private readonly UserProfile _userProfile;
        private readonly ILogger _logger;

        public iPyprflowClient(HttpClient httpClient, UserProfile userProfile, ILogger logger)
        {
            _httpClient = httpClient;
            _userProfile = userProfile;
            _logger = logger;
        }

        public async Task<string> GetAsync(string url)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            return await Request(requestMessage);
        }

        private async Task<string> Request(HttpRequestMessage requestMessage)
        {
            //   _httpClient.BaseAddress = new Uri(_host);
            string url = "someurl";
            HttpRequestMessage requestMessage1 = new HttpRequestMessage(HttpMethod.Get, url);
            return await Request(requestMessage1);

        }
    }
}

