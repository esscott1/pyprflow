using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using workflow.Model;
using workflow.Db;
using Microsoft.AspNetCore.WebUtilities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace workflow.Controllers
{
	[Route("api/[controller]")]
	public class SearchController : Controller
	{
		public SearchController(IWorkflowRepository workflow)
		{
			Repository = workflow;
		}

		[HttpGet("test")]
		public IActionResult Test(string q, string w)
		{
			Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dic =
			new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();

			//var result = Repository.Where<Transaction>(i => i.TrackableName == "SampleDoc1");
			dic = QueryHelpers.ParseQuery(Request.QueryString.ToString());
			var result = w.Split('=')
				.Select((v, i) => new { v, i })
				.GroupBy(x => x.i / 2)
				.ToDictionary(g => g.First().v,
				g => g.Last().v);
			return Json(result);

		}
		public IWorkflowRepository Repository { get; set; }
		// GET: /<controller>/
		[HttpGet]
		public IActionResult Search(string select, 
			string workflowId,
			string trackableId,
			string transactionId,
			string nodeId,
			string start,
			string end)
		{

			Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dic =
				new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
			Console.WriteLine("in the search");
			dic = QueryHelpers.ParseQuery(Request.QueryString.ToString());
			SearchEngine se = new Db.SearchEngine(Repository);
			object result = null;
			
			result = se.Search(dic);

			return Json(result);
		}

		

	
	}
}
