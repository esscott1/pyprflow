using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using workflow.Model;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace workflow.Controllers
{
	[Route("api/[controller]")]
    public class TrackableController : Controller
    {
		public TrackableController(IWorkflowRepository workflow)
		{
			Workflow = workflow;
		}
		public IWorkflowRepository Workflow { get; set; }
		[HttpPost("isunique/{id}")]
		public bool IsUnique(string id)
		{

			return true;
		}
		[HttpGet("newId")]
		public IActionResult NewTrackableId()
		{
			return Json(Guid.NewGuid());
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

		

		[HttpPut("move")]
		public IActionResult Move([FromBody]WorkflowUpdate workflowUpdate)
		{
			var workflow = Workflow.Find(workflowUpdate.WorkflowId);
			if (workflow == null) 
				return NotFound("workflow not found");
			try
			{
				workflow.MoveToNode(workflowUpdate.TrackableId, workflowUpdate.NodeId);
			}
			catch (WorkFlowException ex)
			{
				return Json(ex.Message);
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return new ObjectResult(workflow);
		}

		[HttpPut("movenext")]
		public IActionResult MoveNext([FromBody]WorkflowUpdate workflowUpdate)
		{
			Workflow workflow; string nodeName; string nextNodeName;

			try { workflow = Workflow.Find(workflowUpdate.WorkflowId); }
			catch (Exception ex)
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
		[HttpPut("start")]
		public IActionResult SubmitToWorkflow([FromBody]WorkflowUpdate workflowUpdate)
		{
			Workflow workflow = Workflow.Find(workflowUpdate.WorkflowId);
			KeyValuePair<string, Node> firstNode = workflow.GetFirstNode();
			Trackable t = new Model.Trackable(workflowUpdate.TrackableId);
			firstNode.Value.Trackables.Add(t);
			return Json(t);
		}


		[HttpDelete("remove")]
		public IActionResult RemoveTrackable([FromBody] WorkflowUpdate workflowUpdate)
		{
			Workflow wf = Workflow.Find(workflowUpdate.WorkflowId);
			wf.RemoveItemFromWorkflow(workflowUpdate.TrackableId);
			return Json("tried to delete ID: " + workflowUpdate);
		}
	}
}
