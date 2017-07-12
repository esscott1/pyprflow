using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pyprflow.Workflow.Model;
using pyprflow.Workflow.Search;
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
            "assignedto","submittedby", "etest",
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
		public IActionResult Search(string sEntityType, 
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
            StringValues svEntityType; 
           // EntityType entityType;

            if (!dic.TryGetValue("entitytype", out svEntityType))
                return StatusCode(400, "EntityType must be: " + string.Join(" | ", Enum.GetNames(typeof(EntityType))));

            if (!Enum.TryParse(svEntityType, true, out EntityType entityType))
                 return StatusCode(400, "EntityType must be: " + string.Join(" | ", Enum.GetNames(typeof(EntityType))));
           
            IEnumerable<BaseWorkflowItem> result = null;
            
            SearchRequestParameters srp = new SearchRequestParameters();
            if (dic.TryGetValue("transactiontype", out StringValues sTransactiontype))
                if (Enum.TryParse(sTransactiontype, out Database.Entity.TransactionType tType))
                    srp.transactiontype = tType;

            if (dic.TryGetValue("isactive", out StringValues sIsActive))
                if (Enum.TryParse(sIsActive.ToString().ToLower(), out IsActive tIsActive))
                    srp.isActive = tIsActive;
            
            HashSet<string> commonKeys = new HashSet<string>(srp.Parameters.Keys);
            commonKeys.IntersectWith(dic.Keys);
            foreach(string k in commonKeys)
            {
                srp.Parameters[k] = dic[k];
            }

            SearchRequest request2 = new SearchRequest(srp, entityType);
            SearchEngineContext se1 = new SearchEngineContext(Repository);
            result = se1.Search(request2);
            if (result.Count() == 1)
                return Json(result.First());
            return Json(result);
		}

	}
}
