
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace pyprflow.Cli
{
    [Command(Name = "login", Description = "login to ipyprflow, the login crendentials will be saved locally in the profile")]

    internal class LoginCmd: iPyprflowCmdBase
    {

        [Option(CommandOptionType.SingleValue, ShortName = "u", LongName = "username", Description = "ipyprflow login username", ValueName = "login username", ShowInHelpText = true)]
        public string Username { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "p", LongName = "password", Description = "ipyprflow login password", ValueName = "login password", ShowInHelpText = true)]
        public string Password { get; set; }

        [Option(CommandOptionType.NoValue, LongName = "staging", Description = "ipyprflow staging api", ValueName = "staging", ShowInHelpText = true)]
        public bool Staging { get; set; } = false;

        public LoginCmd(ILogger<LoginCmd> logger, IConsole console, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _console = console;
            _httpClientFactory = clientFactory;
        }
        private iPyprflowCmd Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                Username = Prompt.GetString("iPyprflow Username:", Username);
                Password = SecureStringToString(Prompt.GetPasswordAsSecureString("iPyprflow Password:"));
                Staging = Prompt.GetYesNo("iPyprflow Staging?   ", Staging);
                Profile = Prompt.GetString("User profile name:", Profile);
                OutputFormat = Prompt.GetString("Output format (json|xml|text|table):", OutputFormat);
            }

            try
            {
                var userProfile = new UserProfile()
                {
                    Username = Username,
                    Password = Encrypt(Password),
                    Staging = Staging,
                    OutputFormat = OutputFormat
                };

                if (!Directory.Exists(ProfileFolder))
                {
                    Directory.CreateDirectory(ProfileFolder);
                }

                await File.WriteAllTextAsync($"{ProfileFolder}{Profile}", JsonConvert.SerializeObject(userProfile, Formatting.Indented), UTF8Encoding.UTF8);
        // for login authentication
              //  var token = await iPyprflowClient.GetTokenAsync();

                return 0;

            }
            catch (Exception ex)
            {
                OnException(ex);
                return 1;
            }
        }
    }
}