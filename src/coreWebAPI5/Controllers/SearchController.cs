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
		public IActionResult Search(string q, string id, string nodeid)
		{

			Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dic =
				new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();

			dic = QueryHelpers.ParseQuery(Request.QueryString.ToString());
			SearchEngine se = new Db.SearchEngine(Repository);
			object result = null;
			if (dic.ContainsKey("id"))
				result = se.Search(dic);
			else
				result = se.SearchAll<Trackable>(dic);

			return Json(result);
		}

		private IActionResult SearchById<T>(string id)
		{
			return Json(Repository.Find<T>(id));
		}

		[HttpGet("workflow")]
		public IActionResult SearchWorkflow(string id, string where)
		{
			if (id != null)
				return SearchById<Workflow>(id);
			throw new NotImplementedException(); ;
		}

		[HttpGet("node")]
		public IActionResult SearchNode(string id, string where)
		{
			throw new NotImplementedException(); ;
		}

		[HttpGet("transaction")]
		public IActionResult SearchTransaction(string id, string where)
		{
			if (id != null)
				return SearchById<Transaction>(id);
			throw new NotImplementedException(); ;
		}

		[HttpGet("trackable")]
		public IActionResult SearchTrackable(string id, string where)
		{
			if (id != null)
				return SearchById<Trackable>(id);
			if (where != null)
			{
				SearchEngine se = new Db.SearchEngine(Repository);
				se.SearchAll<Trackable>(where);
			}
			throw new NotImplementedException();
		}
	}
}
