using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using workflow.Model;
using workflow.Db;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace workflow.Controllers
{
	[Route("api/[controller]")]
	public class SearchController : Controller
	{
		public static readonly string[] SearchParameters = { "entitytype","trackableid","transactionid",
			"nodeid",
			"assignmentto",
			"transactiontype",
			"isactive" };
		public static readonly string[] sEntityTypes = { "workflows", "trackables", "transactions" };
		public SearchController(IWorkflowRepository workflow)
		{
			Repository = workflow;
		}

		[HttpGet("test")]
		public IActionResult Test(string q, string w)
		{
			Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dic =
			new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();

			dic = QueryHelpers.ParseQuery(Request.QueryString.ToString());
			var result = w.Split('=')
				.Select((v, i) => new { v, i })
				.GroupBy(x => x.i / 2)
				.ToDictionary(g => g.First().v,
				g => g.Last().v);
			return Json(result);

		}
		public IWorkflowRepository Repository { get; set; }
		[HttpGet]
		public IActionResult Search(string entityType, 
			string workflowId,
			string trackableId,
			string transactionId,
			string nodeId,
			string assignedTo,
			string type,
			string start,
			string end,
			string isActive)
		{
		
			Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dic =
				new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
			Console.WriteLine("in the search Controller");
			
			dic = QueryHelpers.ParseQuery(Request.QueryString.ToString());
			StringValues sEntityType;
			if (dic.TryGetValue("entitytype", out sEntityType))
			{
				if (!sEntityTypes.Contains(sEntityType.ToString()))
					return StatusCode(400, "EntityType must be: "+ String.Join(" | ", sEntityTypes));
			}
			else
				return StatusCode(400, "Must include Entitytype to define type of object for search to return");

			foreach (KeyValuePair<string, StringValues> s in dic.ToArray())
			{
				if(!SearchParameters.Contains(s.Key.ToLower()))
				{
					return StatusCode(400, "bad search parameter provided. " + s.Key);
				}
			}
				//Console.WriteLine("{0} is key {1} is value in querystring",s.Key, s.Value);
			SearchRequest request = new SearchRequest(dic);

			SearchEngine se = new Db.SearchEngine(Repository);
			object result = null;
			result = se.Search(request);
			
			return Json(result);
		}

		

		


	}
}
