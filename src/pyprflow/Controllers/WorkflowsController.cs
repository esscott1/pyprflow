using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using pyprflow.Workflow.Model;



// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace pyprflow.Api.Controllers
{
	[Route("api/[controller]")]
	public class WorkflowsController : Controller
	{
		public WorkflowsController(IWorkflowRepository workflow)
		{
			Repository = workflow;
		}
		public IWorkflowRepository Repository { get; set; }

		[HttpGet]
		public IEnumerable<Workflow.Model.Workflow> GetAll()
		{ return Repository.GetAll<Workflow.Model.Workflow>(); }

		[HttpGet("{id}", Name = "GetWorkflow")]
		public IActionResult GetById(string id)
		{
			var workflow = Repository.Find<Workflow.Model.Workflow>(id);
			if (workflow == null) { return NotFound(id); }
			return Json(workflow);
		}

		[HttpGet("example")]
		public IActionResult GetSample()
		{
			var wkf = new Workflow.Model.Workflow();
			return Json(wkf.GetSample());
		}


		[HttpGet("{workflowId}/orchestrations")]
		public IEnumerable<Orchestration> GetOrchestrations(string workflowId)
		{
			var workflow = Repository.Find<Workflow.Model.Workflow>(workflowId);
			List<Workflow.Model.Orchestration> ol = new List<Workflow.Model.Orchestration>();
			foreach (KeyValuePair<string, Orchestration> kvp in workflow.Orchestrations)
				ol.Add(kvp.Value);
			return ol;
		}

		[HttpGet("{workflowId}/nodes")]
		public IEnumerable<Node> GetNodes(string workflowId)
		{
			var workflow = Repository.Find<Workflow.Model.Workflow>(workflowId);
			List<Workflow.Model.Node> n = new List<Workflow.Model.Node>();
			foreach (KeyValuePair<string, Workflow.Model.Node> kvp in workflow.Nodes)
				n.Add(kvp.Value);
			return n;
		}

		[HttpGet("{workflowId}/orchestrations/{nodeId}")]
		public Orchestration GetNode(int workflowId, string nodeId)
		{
			return Repository.Find<Workflow.Model.Orchestration>(nodeId);
		}

		/// <summary>
		/// undocumented endpoint.  using for development testing only
		/// </summary>
		/// <param name="workflow"></param>
		/// <returns></returns>
		[HttpPost("validate", Name = "ValidateWorkflow")]
		public IActionResult Validate([FromBody] Workflow.Model.Workflow workflow)
		{
			WorkflowValidationMessage message;

			bool valid = workflow.IsValid(out message);
			if (valid)
				return Json(new { valid = true });
			return StatusCode(422, message); //Json(message);

		}

		[HttpPost]
		public IActionResult Create([FromBody] Workflow.Model.Workflow workflow)
		{
			if (workflow == null)
			{
				return BadRequest("workflow was null");
			}
			WorkflowValidationMessage message;
			if (!workflow.IsValid(out message))
				return StatusCode(422, message);
			Console.WriteLine("in controler trying to save {0} with type {1}", workflow.Name, workflow.DerivedType);
			Repository.Add(workflow);

			return CreatedAtRoute("GetWorkflow", new { id = workflow.Name }, Repository);
		}


		[HttpPut("{id}")]
		public IActionResult Update(string id, [FromBody] Workflow.Model.Workflow workflow)
		{
			if (workflow == null || workflow.Name != id)
			{
				return BadRequest();
			}
			var _workflow = Repository.Find<Workflow.Model.Workflow>(id);
			if (_workflow == null)
				return NotFound();
			Repository.Update<Workflow.Model.Workflow>(workflow);
			return new NoContentResult();

		}

		[HttpDelete("{id}")]
		public IActionResult Delete(string id)
		{
			Console.WriteLine("in the delete method of controller");
			Repository.Remove<Workflow.Model.Workflow>(id);
			return Json(String.Format("workflow with workflowItemId {0} is deleted", id));
		}


        [HttpPut("deactivate/{id}")]
        public IActionResult Deactivate(string id)
        {
            Repository.Deactivate<Workflow.Model.Workflow>(id);
            //    var _workflow = Repository.Find<Workflow>(id);
            return Json(String.Format("workflow with workflowItemId {0} is has been soft deleted", id));
        }

        //[HttpPut("deactivate/{id}")]
        //public IActionResult Deactivate(string id)
        //{
        //    Repository.Deactivate<Workflow>(id);
        //    //    var _workflow = Repository.Find<Workflow>(id);
        //    return Json(String.Format("workflow with workflowItemId {0} is has been soft deleted", id));
        //}


        //[HttpPatch("{id}")]
        //public IActionResult UpdatePatch([FromBody] JsonPatchDocument<Workflow> patch, string id)
        //{
        //	Workflow wf = Repository.Find<Workflow>(id);
        //	Workflow patched = Repository.Find<Workflow>(id);
        //	patch.ApplyTo(patched, ModelState);
        //	if (!ModelState.IsValid)
        //		return new BadRequestObjectResult(ModelState);
        //	var model = new
        //	{
        //		orginal = wf,
        //		patched = patched,
        //		operations = patch.Operations
        //	};

        //	return null;

        //}
    }
}
