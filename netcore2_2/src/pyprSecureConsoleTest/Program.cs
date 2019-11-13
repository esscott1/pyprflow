using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace pyprSecureConsoleTest
{
    public class Program
    {
        public static async Task Main()
        {
            Console.Title = "Console ClientCredentials Flow PostBody";

            var response = await RequestTokenAsync();
          // var response = await RequestClientCredTokenAsync();
            //   response.Show();

           // Console.ReadLine();
            await CallServiceAsync(response.AccessToken);
        }
       static async Task<TokenResponse> RequestTokenAsync()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44350");
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientCredentialStyle = ClientCredentialStyle.PostBody,
                GrantType = "password",
                UserName = "scott",
                Password = "password",
                ClientId = "oauthClient",
                ClientSecret = "superSecretPassword",
                Scope = "api"
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        static async Task<TokenResponse> RequestClientCredTokenAsync()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44350");
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientCredentialStyle = ClientCredentialStyle.PostBody,
                ClientId = "oauthClient",
                ClientSecret = "superSecretPassword",
                Scope = "api",
                
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        static async Task CallServiceAsync(string token)
        {
            var baseAddress = "https://localhost:44303";
            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
          
                client.SetBearerToken(token);
                var response = await client.GetStringAsync("weatherforecast/secure");
          
            //  "\n\nService claims:".ConsoleGreen();
            Console.WriteLine(JArray.Parse(response));
            Console.ReadLine();
        }
    }
}
