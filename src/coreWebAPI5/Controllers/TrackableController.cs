using Newtonsoft.Json;
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
			Repository = workflow;
		}
		public IWorkflowRepository Repository { get; set; }
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
			return Repository.GetAll<Trackable>();
		}

		[HttpGet("{id}", Name ="GetTrackable") ]
		public IActionResult GetTrackable(string id)
		{
			Trackable t = Repository.Find<Trackable>(id);
			return Json(t);
		}

		[HttpGet("{trackableId}/transactions")]
		public IEnumerable<Transaction> GetTransactions(string trackableId)
		{
			return Repository.GetAll<Transaction>().Where(t => t.TrackableId2 == trackableId);
		}
		
		[HttpPost]
		public IActionResult CreateTrackable([FromBody] Trackable item)
		{
			// should check for existance and if exist throw error telling to use Put
			var t = Repository.Find<Trackable>(item.Key);
			if (t != null)
				return StatusCode(403, "Trackable already exists in system, use HTTPPatch to update trackable");
			Repository.Add(item);
			return CreatedAtRoute("GetTrackable", new { id = item.Key }, Repository);
		}

		// patch is inappropriate because when an trackable is "moved" we need other information
		// that is not part of the Trackable Object to track the  move.
		// Patch may be useful for partial updates of a trackable so leaving it in for now
	//	[HttpPatch("{id}")]
		//public IActionResult UpdatePatch([FromBody]JsonPatchDocument<Trackable> patch, string id)
		//{
		//	Trackable trackable = Repository.FindTrackable(id);
		//	Trackable patched = Repository.FindTrackable(id);
		//	patch.ApplyTo(patched, ModelState);
		//	if (!ModelState.IsValid)
		//		return new BadRequestObjectResult(ModelState);
		//	var model = new
		//	{
		//		orginal = trackable,
		//		patched = patched,
		//		operations = patch.Operations
		//	};
			
			
		//	var locationUpdates = patch.Operations.Where(o => o.Orchestration.ToLowerInvariant() == "/locations/-");
		//	List<Location> newlocations = new List<Location>();
		//	// getting a list of locations that were sent in the patch.
		//	foreach(Microsoft.AspNetCore.JsonPatch.Operations.Operation<Trackable> o in locationUpdates)
		//	{
		//		string sVar = JsonConvert.SerializeObject(o.value);
		//		var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
		//		Location loc = JsonConvert.DeserializeObject<Location>(sVar, settings);

		//		switch (o.op)
		//		{
		//		case "add":
		//				string nodeId;
		//				if(WorkflowHelper.ExistsInWorkflow(trackable, loc.WorkflowGuid, out nodeId))
		//				{
		//					var wf = Repository.Find(loc.WorkflowGuid);
		//					Movement move;
		//					if(wf.IsMoveValid(nodeId,loc.NodeId, out move))
		//					{
		//						// merge in patch and save the move
		//					}
		//				}
		//			l = (Location)o.value;
		//			return Json(l);
		//			break;
		//		case "replace":
		//			l = (Location)o.value;
		//			return Json(l);
		//			break;

		//		}
				
		//		newlocations.Add(loc);
		//	}
		//	// match those locations with existing locations to check if the "move" is valid
			
		//	foreach(Location l in newlocations)
		//	{

		//		var wf = Repository.Find(l.WorkflowGuid);
		//		Movement move;
		//		if(!wf.IsMoveValid("somewhere",l.NodeId, out move))
		//		{
		//			return StatusCode(403, "move is not valid per workflow rules");
		//		}

		//	}


		//	//string sVar = JsonConvert.SerializeObject(locationUpdates.First().value);
		//	//var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
		//	//Location loc =  JsonConvert.DeserializeObject<Location>(sVar, settings);

		//	return Json(loc);  // the above works.

		//	return Json(locationUpdates.First().value);

		//	foreach (Microsoft.AspNetCore.JsonPatch.Operations.Operation<Trackable> o in locationUpdates)
		//	{
		//		Microsoft.AspNetCore.JsonPatch.Helpers.GetValueResult gvr = new Microsoft.AspNetCore.JsonPatch.Helpers.GetValueResult(o.value, false);
		//		if (o.op == "add")
		//			return Ok(Json(
		//				((Newtonsoft.Json.Linq.JObject)o.value).Property("value").Value						));
		//		else
		//			return Ok(Json("not add"));
		//		Location l;
		//		//switch (o.op)
		//		//{
		//		//	case "add":
		//		//		l = (Location)o.value;
		//		//		return Json(l);
		//		//		break;
		//		//	case "replace":
		//		//		l = (Location)o.value;
		//		//		return Json(l);
		//		//		break;
		//		//	case "remove":
		//		//		l = (Location)o.value;
		//		//		return Json(l);
		//		//		break;
		//		//	case "move":
		//		//		return StatusCode(403, "move operation not supported");
		//		//		break;
		//		//	case "copy":
		//		//		return StatusCode(403, "copy operation not supported");
		//		//		break;
		//		//	case "test":
		//		//		return StatusCode(403, "test operation not supported");
		//		//		break;
		//		//	default:
		//		//		return Json("default case");
		//		//		break;
		//		//}
		//	}
		//	return Ok(model);
		//	//return Json(patch);

		//}
	
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

		[HttpGet("availablemoves")]
		public IEnumerable<Movement> AvailableMoves([FromBody] Trackable item)
		{
			throw new NotImplementedException();
			//var nodeName = item.Location[workflowId];

			//Repository workflow = Repository.Find(workflowId);
			
			//return workflow.Orchestration.Where(p => p.From == nodeName);
			
		}


		[HttpPost("start")]
		public IActionResult SubmitToWorkflow([FromBody]Trackable item, string workflowId)
		{
			//Repository workflow = Repository.Find(workflowId);
			//item.Location.Add(workflowId, workflow.StartingNodeName);
			//Repository.Add(item);
			//return Json(item);
			throw new NotImplementedException();
		}


		[HttpDelete("remove")]
		public IActionResult RemoveTrackable([FromBody] WorkflowAction workflowUpdate)
		{
			Workflow wf = Repository.Find<Workflow>(workflowUpdate.WorkflowId);
			wf.RemoveItemFromWorkflow(workflowUpdate.TrackableId);
			return Json("tried to delete ID: " + workflowUpdate);
		}
	}
}
