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

		[HttpGet("example")]
		public IActionResult GetSample()
		{

			var wkf = new Model.Workflow();
			return Json(wkf.GetSample());

		}
		[HttpGet]
		public IEnumerable<Workflow> GetAll()
		{
			return Workflow.GetAll<Workflow>();
			//return Workflow.GetAll();
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

		[HttpGet("{workflowId}/Node/{nodeId}/trackables")]
		public IEnumerable<Trackable> GetNodeTrackables(string workflowId, string nodeId)
		{
			var ie = Workflow.GetAllTrackable();
			return ie.Where(t => t.Locations.Any(l => l.NodeId == nodeId));
			
		}
		[HttpPost("validate", Name ="ValidateWorkflow")]
		public IActionResult Validate([FromBody] Workflow workflow)
		{
			WorkflowValidationMessage message;

			bool valid = workflow.IsValid(out message);
			if (valid)
				return Json(new { valid = true });
			return StatusCode(422, message); //Json(message);

		}
		[HttpPost]
		public IActionResult Create([FromBody] Workflow workflow)
		{
			if (workflow == null)
			{
				return BadRequest("workflow was null");
			}
			WorkflowValidationMessage message;
			if (!workflow.IsValid(out message))
				return StatusCode(422, message);
			Workflow.Add(workflow);
		
			return CreatedAtRoute("GetWorkflow", new { id = workflow.Key }, Workflow);
		}
		//[HttpPut]
		//public IActionResult StartTrackingItem(string workflowId, [FromBody] ITrackable item)//string workFlowId, [FromBody] ITrackable trackable)
		//{
		//	var workflow = Workflow.Find("_blankKey");
		//	var t = new Model.Trackable("documentToTrack1", "_blankKey");
		//	workflow.AddTrackableToStart(t);
		//	return new ObjectResult(workflow);
		//}
		
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
		[HttpDelete("{id}")]
		public IActionResult Delete(string id)
		{
			var deleted = Workflow.Remove(id);
			if (deleted == null)
				return NotFound();
			return Json(deleted);
		}
		
	}
}
