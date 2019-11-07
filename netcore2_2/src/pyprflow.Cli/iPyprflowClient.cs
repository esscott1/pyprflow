using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace pyprflow.Cli
{
    public class iPyprflowClient
    {
        private readonly HttpClient _httpClient;
        private readonly UserProfile _userProfile;
        private readonly ILogger _logger;
        private readonly String _host;

        public iPyprflowClient(HttpClient httpClient, UserProfile userProfile, ILogger logger )
        {
            _httpClient = httpClient;
            _userProfile = userProfile;
            _logger = logger;
            
          //  _host = configuration["Host:Location"];
        }
        public async Task<string> PostAsync(string url, string body)
        {
            url = _httpClient.BaseAddress.AbsoluteUri + url;
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine("calling " + url);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Add("user-key", "test");
            //requestMessage.Headers.Add("Content-Type", "application/json");
            requestMessage.Content = new StringContent(
                body, Encoding.UTF8, "application/json");

            return await Request(requestMessage);

        }
        public async Task<string> GetAsync(string url)
        {
            url = _httpClient.BaseAddress.AbsoluteUri + url;
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

