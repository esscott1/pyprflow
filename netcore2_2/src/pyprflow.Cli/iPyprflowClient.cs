using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IdentityModel.Client;

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

        public async Task<TokenResponse> RequestTokenAsync()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001/");
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientCredentialStyle = ClientCredentialStyle.PostBody,

                ClientId = "oauthClient",
                ClientSecret = "superSecretPassword",
                Scope = "customAPI.read"
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;

        }
        public async Task<string> GetAsync(string url)
        {
            var response = await RequestTokenAsync();
            var eClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001/")
            };
            try
            {
                eClient.SetBearerToken(response.AccessToken);
                var endResponse = await eClient.GetStringAsync("api/values");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            


            var jsontoken = await PostAsync("values", System.IO.File.ReadAllText("input/cred.json"));
            JObject jo = JObject.Parse(jsontoken);
            var token = (string)jo["token"];


            url = _httpClient.BaseAddress.AbsoluteUri + url;
            _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
         //   "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJuYmYiOjE1NzM1ODAwMzAsImV4cCI6MTU3NDE4NDgzMCwiaWF0IjoxNTczNTgwMDMwfQ.e2V9GYDyFSG4yq5HbDh7dmYMxge_6gbPje38oUmFizw");
         token);
          //  await PostAsync("values", System.IO.File.ReadAllText("input/cred.json")));
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

