using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Threading.Tasks;

namespace pyprflow.Cli
{
    [Command(Name = "pyprflow", ThrowOnUnexpectedArgument = false, OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(LoginCmd),
        typeof(ListTicketCmd),
        typeof(WorkflowCmd))]
    class iPyprflowCmd: iPyprflowCmdBase
    {
        public iPyprflowCmd(ILogger<iPyprflowCmd> logger, IConsole console, IConfiguration configuration)
        {
            _logger = logger;
            _console = console;
            _configuration = configuration;
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return Task.FromResult(0);
        }

        private static string GetVersion()
            => typeof(iPyprflowCmd).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
