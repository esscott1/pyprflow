using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using coreWebAPI5.Model;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace coreWebAPI5.Controllers
{
	[Route("api/[controller]")]
	public class WorkflowController : Controller
	{
		public WorkflowController(IWorkflowRepository workflow)
		{
			Workflow = workflow;
		}
		public IWorkflowRepository Workflow { get; set; }

		[HttpGet]
		public IEnumerable<Workflow> GetAll()
		{
			return Workflow.GetAll();
		}

		

		[HttpGet("{id}", Name = "GetWorkflow")]
		public IActionResult GetById(string id)
		{
			var workflow = Workflow.Find(id);
			if (workflow == null)
			{
				return NotFound(id);
			}
			return Json(workflow);
		}

		[HttpPut("availablemoves")]
		public IEnumerable<string> AvailableMoves([FromBody] WorkflowUpdate workflowUpdate)
		{
			var workflow = Workflow.Find(workflowUpdate.WorkflowId);
			try
			{
				return workflow.FindAvailableNodes(workflowUpdate.TrackableId);
			}
			catch (WorkFlowException ex)
			{

			}
			finally
			{
				
			}
			return new List<string>();


		}
		[HttpPost]
		public IActionResult Create([FromBody] Workflow workflow)
		{
			if (workflow == null)
			{
				return BadRequest();
			}
			Workflow.Add(workflow);
			return CreatedAtRoute("GetWorkflow", new { id = workflow.Key }, Workflow);
		}
		[HttpPut]
		public IActionResult StartTrackingItem(string workflowId, [FromBody] ITrackable item)//string workFlowId, [FromBody] ITrackable trackable)
		{
			var workflow = Workflow.Find("_blankKey");
			var t = new Model.Trackable("documentToTrack1");
			workflow.AddTrackableToStart(t);
			return new ObjectResult(workflow);
		}
		[HttpPut("move")]
		public IActionResult Move([FromBody]WorkflowUpdate workflowUpdate)
		{
			var workflow = Workflow.Find(workflowUpdate.WorkflowId);
			try
			{
				workflow.MoveToNode(workflowUpdate.TrackableId, workflowUpdate.NodeId);
			}
			catch(WorkFlowException ex)
			{
				return Json(ex.Message);
			}
			 return new ObjectResult(workflow);
		}
		[HttpPut("movenext")]
		public IActionResult MoveNext([FromBody]WorkflowUpdate workflowUpdate)
		{
			Workflow workflow; string nodeName; string nextNodeName;
			
			try {  workflow = Workflow.Find(workflowUpdate.WorkflowId); }
			catch(Exception ex)
			{
				return Json(ex.Message);
			}
			try
			{
				//find the NodeName that the item is in
				nodeName = workflow.GetNodeNameItemIsIn(workflowUpdate.TrackableId);
				nextNodeName = workflow.FindNextNodeName(nodeName);
				workflow.MoveToNode(workflowUpdate.TrackableId, nextNodeName);
			}
			catch (Exception ex)
			{
				return Json(ex.InnerException);
			}
			return new ObjectResult(workflow);
			

		}

		[HttpPut("submit")]
		public IActionResult SubmitToWorkflow([FromBody]Trackable Trackable)
		{
			//Workflow workflow = Workflow.Find(workflowUpdate.WorkflowId);
			//KeyValuePair<string, Node> firstNode = workflow.GetFirstNode();
			//firstNode.Value.Trackables.Add(Trackable);


			return NotFound();
		}


		[HttpPut("{id}")]
		public IActionResult Update(string id, [FromBody] Workflow workflow)
		{
			if (workflow == null || workflow.Key != id)
			{
				return BadRequest();
			}
			var _workflow = Workflow.Find(id);
			if (_workflow == null)
				return NotFound();
			Workflow.Update(_workflow);
			return new NoContentResult();

		}
	}
}
