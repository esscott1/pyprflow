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

		[HttpPost("validate")]
		public IActionResult Validate([FromBody] Workflow workflow)
		{
			WorkflowValidationMessage message;

			bool valid = workflow.IsValid(out message);
			if (valid)
				return Json(new { valid = true });
			return Json(message);

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
		[HttpDelete("workflow/{id}")]
		public IActionResult Delete(string id)
		{
			WorkflowRepository rep = new WorkflowRepository();
			var deleted = rep.Remove(id);
			if (deleted == null)
				return NotFound();
			return Json(deleted);
		}
		//[HttpDelete("trackable/delete")]
		//public IActionResult RemoveTrackable([FromBody] WorkflowUpdate workflowUpdate)
		//{
		//	Workflow wf = Workflow.Find(workflowUpdate.WorkflowId);
		//	wf.RemoveItemFromWorkflow(workflowUpdate.TrackableId);
		//	return Json("tried to delete ID: " + workflowUpdate);
		//}
	}
}
