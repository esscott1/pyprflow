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
				return NotFound();
			}
			return new ObjectResult(workflow);
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
			var t = new Model.Fakes.TrackableDocument("documentToTrack1");
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
	}
}
