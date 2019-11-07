﻿using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace pyprflow.Cli
{
    [Command(Name = "list-tickets", Description = "list ipyprflow tickets")]
    internal class ListTicketCmd : iPyprflowCmdBase
    {
        [Option(CommandOptionType.SingleValue, ShortName = "start", LongName = "start-date", Description = "start date", ValueName = "start date", ShowInHelpText = true)]
        public DateTime? StartDate { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "end", LongName = "end-date", Description = "end date", ValueName = "end date", ShowInHelpText = true)]
        public DateTime? EndDate { get; set; }
        public ListTicketCmd(ILogger<ListTicketCmd> logger, IConsole console, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _console = console;
            _httpClientFactory = clientFactory;

        }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            try
            {
                //if (!StartDate.HasValue)
                //{
                //    StartDate = DateTime.Parse(Prompt.GetString("start date:", DateTime.Now.ToShortDateString()));
                //}

                //if (!EndDate.HasValue)
                //{
                //    EndDate = DateTime.Parse(Prompt.GetString("end date:", DateTime.Now.ToShortDateString()));
                //}
                // var url = $"/api/tickets?filter[where][ticketDate][gt]={StartDate.Value.ToUniversalTime().ToString("o")}&filter[where][ticketDate][lt]={EndDate.Value.ToUniversalTime().ToString("o")}";


                var url = "https://pyprflow.io/api/values";

                var tickets = await iPyprflowClient.GetAsync(url);
                Console.WriteLine("got the tickets");
                //var tickets = String.Empty;
                if (string.IsNullOrEmpty(tickets)) return 0;

                //if (StartDate == EndDate)
                //{
                //    FileNameSuffix = $"{StartDate.Value.ToString("yyyyMMdd")}";
                //}
                //else
                //{
                //    FileNameSuffix = $"{StartDate.Value.ToString("yyyyMMdd")}-{EndDate.Value.ToString("yyyyMMdd")}";
                //}
                //string sTestInfo = "some data from the url";

                //OutputJson(sTestInfo, "tickets", "ticket");
                OutputJson(tickets, "tickets", "ticket");

                //   Console.ReadLine();
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