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
	public class TransactionController : Controller
	{
		public TransactionController(IWorkflowRepository workflow)
		{
			Workflow = workflow;
		}
		public IWorkflowRepository Workflow { get; set; }
		[HttpGet("example")]
		public IActionResult GetSample()
		{
			Transaction t = new Transaction();
			t.Comment = "Submitting to workflow";
			t.Key = "SampleTransaction1";
			t.NewNodeId = "SampleNode1";
			t.PreviousNodeId = null;
			t.Submitter = new User() { Email = "Sample.User@somewhere.com" };
			t.TrackableId = "SampleDoc1";
			t.WorkflowId = "SampleWorkflow1";
			t.type = TransactionType.Move;
			var d = DateTime.Now;
			t.TransActionTime = DateTime.Now;
			return Json(t);

		}
		[HttpGet]
		public IActionResult GetAll()
		{
			//return Json("stuff");
			return Json(Workflow.GetAllTransactions());
		}
	//	GET: api/values
	   [HttpGet("{id}",Name = "GetTransaction")]
		public IActionResult GetTransaction(string id)
		{
			return Json(Workflow.FindTransaction(id));
		}

		// only allow Posts of transactions as they are not editable
		[HttpPost]
		public IActionResult SubmitTransaction([FromBody] Transaction trans)
		{
			
			if (trans == null)
			{ return BadRequest("trans is null"); }
			try
			{
				if (Workflow.GetAllTransactions().FirstOrDefault(t => t.WorkflowId == trans.WorkflowId &&
				 t.NewNodeId == trans.NewNodeId && t.PreviousNodeId == trans.PreviousNodeId) != null)
					return StatusCode(403, "transaction already exists");

				var trackable = Workflow.FindTrackable(trans.TrackableId);
				if (trackable != null) // trackable exists
				{
					if(trans.PreviousNodeId != null) // this is a starting request
						if (!trackable.Locations.Exists(l => l.WorkflowId == trans.WorkflowId && l.NodeId == trans.PreviousNodeId))
							return StatusCode(403, "trackable is not in the starting position for this move request");

					var workflow = Workflow.Find(trans.WorkflowId); Movement move;
					if (!workflow.IsMoveValid(trans.PreviousNodeId, trans.NewNodeId, out move))
						return StatusCode(403, "requested move is not valid in the designated workflow");

					Workflow.Add(trans);
					trackable.Locations.Remove(new Location() { WorkflowId = trans.WorkflowId, NodeId = trans.PreviousNodeId });
					trackable.Locations.Add(new Location() { WorkflowId = trans.WorkflowId, NodeId = trans.NewNodeId });
				}
				else
					return StatusCode(403, "trackable you are trying to move does not exist");

				return CreatedAtRoute("GetTransaction", new { id = trans.Key }, Workflow);
				
			}
			catch(Exception ex)
			{ return (StatusCode(500, ex.InnerException)); }

		}

		
		}

	
}
