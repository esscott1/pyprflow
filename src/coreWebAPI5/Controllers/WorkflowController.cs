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
	public class WorkflowsController : BaseController
	{
		public WorkflowsController(IWorkflowRepository workflow)
		{
			Repository = workflow;
		}

		[HttpGet]
		public IEnumerable<Workflow> GetAll()
		{ return Repository.GetAll<Workflow>(); }

		[HttpGet("{id}", Name = "GetWorkflow")]
		public IActionResult GetById(string id)
		{
			var workflow = Repository.Find<Workflow>(id);
			if (workflow == null) { return NotFound(id); }
			return Json(workflow);
		}

		[HttpGet("example")]
		public IActionResult GetSample()
		{
			var wkf = new Model.Workflow();
			return Json(wkf.GetSample());
		}

		[HttpGet("{workflowId}/Node/{nodeId}/trackables")]
		public IEnumerable<Trackable> GetNodeTrackables(string workflowId, string nodeId)
		{
			IEnumerable<Trackable> ie = Repository.GetAll<Trackable>();
			return ie.Where(t => t.Locations.Any(l => (l.NodeId == nodeId) && (l.WorkflowId==workflowId)));
		}

		[HttpGet("{workflowId}/orchestrations")]
		public IEnumerable<Orchestration> GetOrchestrations(string workflowId)
		{
			var workflow = Repository.Find<Workflow>(workflowId);
			List<Orchestration> ol = new List<Orchestration>();
			foreach (KeyValuePair<string, Orchestration> kvp in workflow.Orchestrations)
				ol.Add(kvp.Value);
			return ol;
		}

		[HttpGet("{workflowId}/orchestrations/{orchestrationId}")]
		public Orchestration GetOrchestration(string workflowId, string orchestrationId)
		{
			return Repository.Find<Orchestration>(orchestrationId);
		}

		[HttpGet("{workflowId}/nodes")]
		public IEnumerable<Node> GetNodes(string workflowId)
		{
			var workflow = Repository.Find<Workflow>(workflowId);
			List<Node> n = new List<Node>();
			foreach (KeyValuePair<string, Node> kvp in workflow.Nodes)
				n.Add(kvp.Value);
			return n;
		}

		[HttpGet("{workflowId}/orchestrations/{nodeId}")]
		public Orchestration GetNode(string workflowId, string nodeId)
		{
			return Repository.Find<Orchestration>(nodeId);
		}

		/// <summary>
		/// undocumented endpoint.  using for development testing only
		/// </summary>
		/// <param name="workflow"></param>
		/// <returns></returns>
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
			Repository.Add(workflow);
		
			return CreatedAtRoute("GetWorkflow", new { id = workflow.WorkflowItemId }, Repository);
		}
		
		
		[HttpPut("{id}")]
		public IActionResult Update(string id, [FromBody] Workflow workflow)
		{
			if (workflow == null || workflow.Key != id)
			{
				return BadRequest();
			}
			var _workflow = Repository.Find<Workflow>(id);
			if (_workflow == null)
				return NotFound();
			Repository.Update<Workflow>(workflow);
			return new NoContentResult();

		}
		[HttpDelete("{id}")]
		public IActionResult Delete(string id)
		{
			Repository.Remove<Workflow>(id);
			return Json(String.Format("workflow with workflowItemId {0} is deleted", id));
		}
		
	}
}
