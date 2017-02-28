using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
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
		[HttpGet("example")]
		public IActionResult GetExample()
		{
			string name = "SampleDoc1";
			Trackable t = new Trackable(name);
			t.Key = name;
			t.TrackableId = name;
			t.Locations.Add(new Location() { WorkflowId = "SampleWorkflow", NodeId = "SampleNode1" });
			return Json(t);

		}
		[HttpGet]
		public IEnumerable<Trackable> GetAll()
		{
			//return Json("works");
			return Workflow.GetAllTrackable();
		}

		[HttpGet("{id}", Name ="GetTrackable") ]
		public IActionResult GetTrackable(string id)
		{
			Trackable t = Workflow.FindTrackable(id);
			return Json(t);
		}

		
		[HttpPost]
		public IActionResult CreateTrackable([FromBody] Trackable item)
		{
			// should check for existance and if exist throw error telling to use Put
			var t = Workflow.FindTrackable(item.Key);
			if (t != null)
				return StatusCode(403, "Trackable already exists in system, use HTTPPatch to update trackable");
			Workflow.Add(item);
			return CreatedAtRoute("GetTrackable", new { id = item.Key }, Workflow);
		}

		[HttpPatch("{id}")]
		public IActionResult UpdatePatch([FromBody]JsonPatchDocument<Trackable> patch, string id)
		{
			Trackable trackable = Workflow.FindTrackable(id);
			Trackable patched = Workflow.FindTrackable(id);
			patch.ApplyTo(patched, ModelState);
			if (!ModelState.IsValid)
				return new BadRequestObjectResult(ModelState);
			var model = new
			{
				orginal = trackable,
				patched = patched
			};
			return Ok(model);
			//return Json(patch);

		}
		[HttpPut]
		public IActionResult Update([FromBody] Trackable item)
		{
			Trackable current = Workflow.FindTrackable(item.Key);
			foreach(Location l in item.Locations)
			{
				foreach(Location lold in current.Locations)
				{
					

				}


			}
			//if (current.Move(item)) ;
			//Workflow.Update(item);
			return Json("not sure what to do here");
		}
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
		public IEnumerable<Movement> AvailableMoves([FromBody] Trackable item)
		{
			throw new NotImplementedException();
			//var nodeName = item.Location[workflowId];

			//Workflow workflow = Workflow.Find(workflowId);
			
			//return workflow.path.Where(p => p.From == nodeName);
			
		}

		[HttpPut("move")]
		public IActionResult Move([FromBody]Trackable item, string workflowId)
		{
			Trackable currentItem = Workflow.FindTrackable(item.Key);
			var workflow = Workflow.Find(workflowId);

			if (workflow == null) 
				return NotFound("workflow not found");
			try
			{
			//	workflow.MoveTrackable(currentItem, item.Location[workflowId]);
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
		public IActionResult MoveNext([FromBody]Trackable item, string workflowId)
		{
			//var currentNode = item.Location[workflowId];
			//var workflow = Workflow.Find(workflowId);
			//var movement = workflow.path.Where(p => p.From == currentNode).First();

			//item.Location.Remove(workflowId);
			//item.Location.Add(workflowId, movement.To);
			//item.MoveHistory.Add(new ExecutedMove(movement) { ExecutionTime = DateTime.Now });
			//return Json(item);
			throw new NotImplementedException();
			
		}

		[HttpPost("start")]
		public IActionResult SubmitToWorkflow([FromBody]Trackable item, string workflowId)
		{
			//Workflow workflow = Workflow.Find(workflowId);
			//item.Location.Add(workflowId, workflow.StartingNodeName);
			//Workflow.Add(item);
			//return Json(item);
			throw new NotImplementedException();
		}


		[HttpDelete("remove")]
		public IActionResult RemoveTrackable([FromBody] WorkflowAction workflowUpdate)
		{
			Workflow wf = Workflow.Find(workflowUpdate.WorkflowId);
			wf.RemoveItemFromWorkflow(workflowUpdate.TrackableId);
			return Json("tried to delete ID: " + workflowUpdate);
		}
	}
}
