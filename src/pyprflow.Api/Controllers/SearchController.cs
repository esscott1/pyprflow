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
	public class SearchController : Controller
	{
		public static readonly string[] SearchParameters = { "entitytype","trackableid","transactionid","workflowid",
			"nodeid",
            "assignedto",
			"transactiontype",
			"isactive" };
		public static readonly string[] sEntityTypes = { "workflows", "trackables", "transactions","trackablesenh" };
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
					return StatusCode(400, "EntityType must be: " + String.Join(" | ", sEntityTypes));
			}
			else
				return StatusCode(400, "Must include Entitytype to define type of object for search to return");

            IEnumerable<BaseWorkflowItem> result = null;



            // route search for simple get all of one type of item
            if (dic.Count == 1) // no clause conditions
            {
                switch (dic["entitytype"])
                {
                    case "workflows":
                        result = new WorkflowsController(Repository).GetAll();
                        break;
                    case "trackables":
                        result = new TrackablesController(Repository).GetAll();
                        break;
                    case "transactions":
                        result = new TransactionsController(Repository).GetAll();
                        break;
                    case "trackablesenh":
                        return StatusCode(400, "trackablesenh entityType is only supported for complex searches");
                    default:
                        return StatusCode(400, "non-supported entityType provided");

                }
                return Json(result);
                // this is return all of something active or inactive
            }

            // validating search parameters
            foreach (KeyValuePair<string, StringValues> s in dic)
            {
                if (!SearchParameters.Contains(s.Key.ToLower()))
                {
                    return StatusCode(400, "bad search parameter provided. " + s.Key);
                }
            }

            // route search for searching for an item by it's ID.
            if (dic.Count == 2)
            {
                if (sEntityType == "workflows" && dic.ContainsKey("workflowid"))
                    return new WorkflowsController(Repository).GetById(dic["workflowid"]);
                if (sEntityType == "trackables" && dic.ContainsKey("trackableid"))
                    return new TrackablesController(Repository).GetById(dic["trackableid"]);
                if (sEntityType == "transactions" && dic.ContainsKey("transactionid"))
                    return new TransactionsController(Repository).GetById(dic["transactionid"]);
            }

            // routing complex searches to search engine with a search request
            
			SearchRequest request = new SearchRequest(dic);
           
            
			SearchEngine se = new SearchEngine(Repository);
            

            result = se.Search(request);
            if (result.Count() == 1)
                return Json(result.First());
			return Json(result);
		}

		

		


	}
}
