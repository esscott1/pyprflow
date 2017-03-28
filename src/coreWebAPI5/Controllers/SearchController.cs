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
		public IWorkflowRepository Repository { get; set; }
		// GET: /<controller>/
		[HttpGet]
		public IActionResult Search(string q, string id)
		{
			
			Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dic = 
				new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();

			dic = QueryHelpers.ParseQuery(Request.QueryString.ToString());
			SearchEngine se = new Db.SearchEngine(Repository);

			var result = se.Search(dic);
			
			return Json(result);
		}
	}
}
