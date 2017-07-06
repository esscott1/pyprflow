using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pyprflow.Workflow.Model;
using pyprflow.Workflow.Db;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace pyprflow.Api.Controllers
{
    [Route("api/[controller]")]
    public class ReportsController : Controller
    {
        //public static readonly string[] SearchParameters = { "entitytype","reporttype","transactionid","workflowid",
        //    "nodeid",
        //    "assignedto","submittedby", "etest",
        //    "transactiontype",
        //    "isactive" };
        //public static readonly string[] sEntityTypes = { "report" };
        public ReportsController(IWorkflowRepository workflow)
        {
            Repository = workflow;
        }


        public IWorkflowRepository Repository { get; set; }
        [HttpGet]
        public IActionResult AgingReport(string id)
        {
            return Reports(id);
        }
        [HttpGet("{id}")]
        public IActionResult Reports(string id)
        {
            pyprflow.Workflow.Model.Reports.ReportEngineContext context = new Workflow.Model.Reports.ReportEngineContext();
            return Json(context.Run(new Workflow.Model.Reports.ReportRequest(id)));

        }
		


	}
}
