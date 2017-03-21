using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using workflow.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace workflow.Controllers
{

	[Route("api/[controller]")]
	public class TransactionController : BaseController
	{
		public TransactionController(IWorkflowRepository workflow)
		{
			Repository = workflow;
		}
	//	public IWorkflowRepository Repository { get; set; }
		[HttpGet("example")]
		public IActionResult GetSample()
		{
			Transaction t = new Transaction();
			t.Comment = "Submitting to workflow";
			//t.Name = "SampleTransaction1";
			t.NewNodeId = "SampleNode1";
			t.PreviousNodeId = null;
			t.Submitter = new User() { Email = "Sample.User@somewhere.com" };
			t.TrackableName = "SampleDoc1";
			t.WorkflowName = "SampleWorkflow1";
			t.type = TransactionType.Move;
			var d = DateTime.Now;
			t.TransActionTime = DateTime.Now;
			t.Name = "SampleTransaction1";
			return Json(t);

		}
		[HttpGet]
		public IActionResult GetAll()
		{
			//return Json("stuff");
			return Json(Repository.GetAll<Transaction>());
		}
	//	GET: api/values
	   [HttpGet("{id}",Name = "GetTransaction")]
		public IActionResult GetTransaction(string id)
		{
			return Json(Repository.Find<Transaction>(id));
		}

		// only allow Posts of transactions as they are not editable
		[HttpPost]
		public IActionResult SubmitTransaction([FromBody] Transaction trans)
		{
			
			if (trans == null)
			{ return BadRequest("trans is null"); }
			try
			{
				if(Repository.Find<Transaction>(trans.Name)!=null)
					return StatusCode(400, "transaction already exists");

				if (Repository.Find<Trackable>(trans.TrackableName) == null)
					return StatusCode(400, "The Trackable does not exist in the system");

				var workflow = Repository.Find<Workflow>(trans.WorkflowName);
				if(workflow == null)
					return StatusCode(400, "The Workflow does not exist in the system");
				
				Console.WriteLine("found workflow: " + workflow.WorkflowName);
				if (!workflow.IsMoveValid(trans))
					return StatusCode(403, "requested move is not valid in the designated workflow");

				Console.WriteLine("move is valid in the workflow");
				Repository.Add(trans);
				Repository.Track(trans);
				return CreatedAtRoute("GetTransaction", new { id = trans.Name }, Repository);
				
			}
			catch(Exception ex)
			{ return (StatusCode(500, ex.InnerException)); }

		}

		
		}

	
}
