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
            Console.WriteLine("calling " +url);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            return await Request(requestMessage);
        }

        private async Task<string> Request(HttpRequestMessage requestMessage)
        {

            // string _host = "https://pyprflow.io/api/values";
            //  _httpClient.BaseAddress = new Uri(_host);
            // string url = "someurl";
            //  HttpRequestMessage requestMessage1 = new HttpRequestMessage(HttpMethod.Get, _host);
            // return await _httpClient.SendAsync(requestMessage);
            var response =  await _httpClient.SendAsync(requestMessage);

            
            if(response.IsSuccessStatusCode)
            {

                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return $"StatusCode: {response.StatusCode}";
            }
         //   return await Request(requestMessage);

        }
    }
}

