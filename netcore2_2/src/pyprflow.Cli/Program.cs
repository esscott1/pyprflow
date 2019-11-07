using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;



namespace pyprflow.Cli
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;
        private static async Task<int> Main(string[] args)
        {
            string sAppSettings = $"\\appSettings.json";
            string sAppSettings2 = $"appSettings.json";
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(env) || env.ToLower() == "production")
            {
                env = "Production";
            }
            else
            {
                env = "Development";
                
                sAppSettings = $"\\appSettings.Development.json";
                sAppSettings2 = $"appSettings.Development.json";
            }
            Console.WriteLine($"Bootstrapping applications using Environment {env}");
            var Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + sAppSettings, optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();


         //   var Configuration = ConfigurationBuilder.Build();

            Log.Logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(Configuration)
                   .Enrich.FromLogContext()
                   .CreateLogger();

            //   var ebuilder = CreateHostBuilder(args);
            //  var builder = CreateHostBuilder(args);
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    hostingContext.HostingEnvironment.EnvironmentName = env;
                    config.AddJsonFile(sAppSettings2, optional: true);
                    config.AddEnvironmentVariables();
                    if (args != null)
                    { config.AddCommandLine(args); }
                }
                )
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(config =>
                    {
                        config.ClearProviders();
                        config.AddProvider(new SerilogLoggerProvider(Log.Logger));
                    });

                    services.AddHttpClient("pyprflow", c =>
                    {
                        c.BaseAddress = new Uri(hostContext.Configuration["Host:Location"]);
                    });


                    services.Add(new ServiceDescriptor(typeof(IConfiguration), Configuration));
                    //   services.AddSingleton(Configuration);

                });
            try
            {
                return await builder.RunCommandLineApplicationAsync<iPyprflowCmd>(args);
                //    var eArgs = new String[] { "workflow" , "--describe"};
               // var eArgs = new String[] { "workflow describe expense-sample1" };
               // int i = await builder.RunCommandLineApplicationAsync<iPyprflowCmd>(eArgs);
               // Console.Read();
                
               // return i;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        private static IHostBuilder CreateHostBuilder(string [] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging(config =>
                {
                    config.ClearProviders();
                    config.AddProvider(new SerilogLoggerProvider(Log.Logger));
                });
                services.AddHttpClient(hostContext.HostingEnvironment.EnvironmentName, c =>
                {
                    c.BaseAddress = new Uri("http://localhost:5000");
                });
                services.Add(new ServiceDescriptor(typeof(IConfiguration), _configuration));
                services.AddSingleton(_configuration);

            });


    }
}