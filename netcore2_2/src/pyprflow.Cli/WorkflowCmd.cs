
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
    [Command("add", Description = "adds workflows")]
    internal class Add : iPyprflowCmdBase
    {
        public Add(ILogger<WorkflowCmd> logger, IConsole console, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _console = console;
            _httpClientFactory = clientFactory;
        }
        [Option(CommandOptionType.SingleValue,
            ShortName = "filelocation",
            LongName = "filelocation",
            Description = "Json file that represents the workflow",
            ValueName = "filelocation", ShowInHelpText = true)]
        public string FileLocation { set; get; }
        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            if (String.IsNullOrWhiteSpace(FileLocation) || String.IsNullOrEmpty(FileLocation))
                FileLocation = Prompt.GetString("File Location", "/input/simple-sample1.json");
            var url = "workflows";
            //var result = await iPyprflowClient.PostAsync(url,app.Arguments);

            // _console.WriteLine("in the list subcommand.");
            var jsonworkflow = System.IO.File.ReadAllText(FileLocation);
            OutputToConsole("--- Workflow Names ---");
            try
            {
                //JObject o = JObject.Parse(result);
                //JArray a = (JArray)o["names"];

                var result = await iPyprflowClient.PostAsync(url, jsonworkflow);
                //IList<string> names = a.ToObject<IList<string>>();
                //foreach (string name in names)
                //{
                //    OutputToConsole(name);

                //}
                OutputJson(result, "workflow", "workflow");
                return 0;
            }
            catch (Exception ex)
            {
                OnException(ex);
                return 1;

            }

        }
    }


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

        [Option(CommandOptionType.NoValue, ShortName = "s", LongName = "sample", Description = "List Workflows from sample inventory", ValueName = "samples", ShowInHelpText = true)]
        protected bool samples { get; set; } = false;
        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            string url = "workflows/list";
            if (samples)
                url = "workflows/list/samples";
            var result = await iPyprflowClient.GetAsync(url);

            // _console.WriteLine("in the list subcommand.");
            OutputToConsole("--- Workflow Names ---");
            try
            {
                JObject o = JObject.Parse(result);
                JArray a = (JArray)o["names"];
                IList<string> names = a.ToObject<IList<string>>();
                foreach (string name in names)
                {
                    OutputToConsole(name);

                }
                OutputJson(result, "workflow", "workflow");
                return 0;
            }
            catch (Exception ex)
            {
                OnException(ex);
                return 1;

            }
           
        }
    }


    [Command(Name = "workflow", Description = "list ipyprflow workflows")]
    [Subcommand(
        typeof(Add),
    //    typeof(DeleteCmd),
        typeof(List))]
    internal class WorkflowCmd: iPyprflowCmdBase
    {
        [Option(CommandOptionType.SingleValue, ShortName ="describe", LongName ="describe", Description ="describe the workflow", ValueName ="describe", ShowInHelpText = true)]
        public string WorkflowName {get;set;}
        [Option(CommandOptionType.NoValue, ShortName = "s", LongName = "sample", Description = "List Workflows from sample inventory", ValueName = "samples", ShowInHelpText = true)]
        protected bool samples { get; set; } = false;
        public WorkflowCmd(ILogger<WorkflowCmd> logger, IConsole console, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _console = console;
            _httpClientFactory = clientFactory;
        }
        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            try
            {
                if (String.IsNullOrEmpty(WorkflowName) || String.IsNullOrWhiteSpace(WorkflowName))
                {
                    WorkflowName = Prompt.GetString("Workflow name:", "expense-sample1");
                }
                if (String.IsNullOrEmpty(WorkflowName) || String.IsNullOrWhiteSpace(WorkflowName))
                {
                    OutputError("Workflow name is required.. run >'pyprflow workflow list' for a list of available workflows to describe.");
                    return 1;
                }
                
                var url = $"search?entitytype=workflows&workflowid={WorkflowName}";
                if (samples)
                    url = $"workflows/sample/{WorkflowName}";
                var result = await iPyprflowClient.GetAsync(url);

               // _console.WriteLine("You must specify at a subcommand.");
                OutputJson(result, "workflow", "workflow");
                return 0;
            }
            catch(Exception ex)
            {
                OnException(ex);
                return 1;
            }
            
        }
        
       

       
    }
}
