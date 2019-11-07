
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace pyprflow.Cli
{

    [Command("list", Description = "Lists workflows")]
    internal class List : iPyprflowCmdBase
    {
        public List(ILogger<WorkflowCmd> logger, IConsole console, IHttpClientFactory clientFactory)  
        {
            _logger = logger;
            _console = console;
            _httpClientFactory = clientFactory;
        }
        [Option(Description = "Show all Workflow Names")]
        public bool All { get; }
        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
          
            var url = "workflows/list";
            var result = await iPyprflowClient.GetAsync(url);

            // _console.WriteLine("in the list subcommand.");
            OutputToConsole("--- Workflow Names ---");
            JObject o = JObject.Parse(result);
            JArray a =(JArray)o["name"];
            IList<string> names = a.ToObject<IList<string>>();
            foreach (string name in names) {
                OutputToConsole(name);
               
                    }
            OutputJson(result, "workflow", "workflow");
            return 1;
        }
    }


    [Command(Name = "workflow", Description = "list ipyprflow workflows")]
    [Subcommand(
    //    typeof(AddCmd),
    //    typeof(DeleteCmd),
        typeof(List))]
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
            var url = $"search?entitytype=workflows&workflowid={WorkflowName}";
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

       
    }
}
