
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
    [Command(Name = "workflow", Description = "list ipyprflow workflows")]
    //[Subcommand(
    //    typeof(AddCmd),
    //    typeof(DeleteCmd),
    //    typeof(List))]
    internal class WorkflowCmd: iPyprflowCmdBase
    {
        [Option(CommandOptionType.SingleValue, ShortName ="describe", LongName ="describe", Description ="describe the workflow", ValueName ="describe", ShowInHelpText = true)]
        public string WorkflowName {get;set;}
        public WorkflowCmd(ILogger<WorkflowCmd> logger, IConsole console, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _console = console;
            _httpClientFactory = clientFactory;
        }
        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            if(String.IsNullOrEmpty(WorkflowName) || String.IsNullOrWhiteSpace(WorkflowName))
            {
                WorkflowName = Prompt.GetString("Workflow name:", "expense-sample1");
            }
            
            var url = "search?entitytype=workflows&workflowid=SampleWorkflow1";

            var result = await iPyprflowClient.GetAsync(url);

            _console.WriteLine("You must specify at a subcommand.");
            OutputJson(result, "workflow", "workflow");
            return 1;
        }


        [Command("describe", Description = "Describes workflow")]
        private class DescribeCmd : WorkflowCmd{
            public DescribeCmd(ILogger<WorkflowCmd> logger, IConsole console, IHttpClientFactory clientFactory):
                base(logger,  console,  clientFactory)
                    { }

            private async Task<int> OnExecute(IConsole console)
            {
                var url = "search?entitytype=workflow?workflowid=expe";
              
                var result = await base.iPyprflowClient.GetAsync(url);

                return 1;
            }
        
        }

        [Command("add", Description = "Adds a workflow")]
        private class AddCmd {
            private int OnExecute(IConsole console)
            {
                console.Error.WriteLine("You must specify an action. See --help for more details.");
                return 1;
            }
        }

        [Command("delete", Description = "Deletes a workflow")]
        private class DeleteCmd {
            private int OnExecute(IConsole console)
            {
                console.Error.WriteLine("You must specify an action. See --help for more details.");
                return 1;
            }
        }

        [Command("list", Description = "Lists workflows")]
        private class List {
            private int OnExecute(IConsole console)
            {
                console.Error.WriteLine("You must specify an action. See --help for more details.");
                return 1;
            }
        }
    }
}
