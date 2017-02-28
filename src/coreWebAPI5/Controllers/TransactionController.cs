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
		[HttpPost]
		public IActionResult SubmitTransaction([FromBody] Transaction trans)
		{
			string result;
			if (trans == null)
			{ return BadRequest("trans is null"); }
			try
			{
			Workflow.Add(trans);
			}
			catch (Exception ex)
			{
				result = ex.Message;
				return Json(result);
			}
			CreatedAtRouteResult response = CreatedAtRoute("GetTransaction", new { id = trans.Key }, Workflow);
			response.StatusCode = 303;
			return response;

		}

		//[Route("api/workflow/{workflowId}/trackable/{trackableId}/transaction/")]
		//[HttpPut]
		//public IActionResult Create([FromBody] Transaction item, 
		//	string workflowId, string trackableId)
		//{
		//	Trackable t = Workflow.FindTrackable(trackableId);
		//	Model.Workflow wf = Workflow.Find(workflowId);
		//	Movement move;
		//	bool valid = wf.IsMoveValid(item.PreviousNodeId, item.NewNodeId, out move);
		//	if(valid)
		//	{
		//		t.CurrentLocation = new Location() { NodeId = item.NewNodeId, WorkflowId = workflowId };
		//		item.type = TransactionType.Move;
		//		Workflow.Add(item);
		//	}

		//	var response = CreatedAtRoute("GetTransaction", new { id = item.Key }, Workflow);
		//	response.StatusCode = 303;
		//	return response;

		//	//return CreatedAtRoute(("GetTransaction", new { id = item.Key }, Workflow);
		//}

	}
}
