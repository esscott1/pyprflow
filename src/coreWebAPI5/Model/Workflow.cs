using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace workflow.Model
{
	public class Workflow
	{
		private static string TestStoreCategoryKey = "WorkFlow_ProofOfConcept";
		public Guid WorkflowId { get; private set; }
		public string Key { get; set; }
		public string WorkflowName { get; private set; }

		internal string StartingNodeName  {get;set;}
		internal string EndingNodeName { get; set; }

		[JsonProperty]
		internal Dictionary<string, Node> Nodes;
	
		[JsonProperty]
		internal List<Movement> path;

		private static IWorkflowRepository wfr { get; set; }
		public Workflow(string workflowName)
		{
			if (WorkflowId == Guid.Empty)
			{
				WorkflowId = Guid.NewGuid();
			}
			DemoWorkflow(workflowName);
		}


		private void DemoWorkflow(string workflowName)
		{
			WorkflowName = workflowName;
			Nodes = new Dictionary<string, Node>();
			path = new List<Movement>();
			if (workflowName == "_blank")
			{
				Key = "_blankKey";
				path.Add(new Movement() { From = "Step1", To = "Step2" });
				path.Add(new Movement() { From = "Step2", To = "Step3" });
				path.Add(new Movement() { From = "Step3", To = "Step4" });
				Nodes.Add("Step1", new Node("Step1") { IsStart = true });
				Nodes.Add("Step2", new Node("Step2"));
				Nodes.Add("Step3", new Node("Step3"));
				Nodes.Add("Step4", new Node("Step4") { IsEnd = true });
			}
		}

		private bool CanExitNode(string nodeName)
		{
			foreach(Movement m in path)
			{
				if (m.From == nodeName)
					return true;
			}
			return false;
		}

		private bool CanEnterNode(string nodeName)
		{
			foreach (Movement m in path)
			{
				if (m.To == nodeName)
					return true;
			}
			return false;

		}
		internal bool IsValid(out WorkflowValidationMessage message)
		{
			message = new WorkflowValidationMessage();
			List<string> NodesWithoutProperPaths = new List<string>();
			// all middle nodes can be moved into and out of 
			foreach(KeyValuePair<string, Node> kvp in Nodes)
			{
				if (kvp.Value.IsStart) { message.HasStart = true; }
				else {
					if (!CanEnterNode(kvp.Key))
						message.UnreachableNodeNames.Add(kvp.Key); 
					}
				if (kvp.Value.IsEnd) {	message.HasEnd = true; }
				else
				{
					if (!CanExitNode(kvp.Key))
						message.DeadEndNodeNames.Add(kvp.Key);
				}
			}
			if (message.Valid)
				return true;
			return false;
		}


		[JsonConstructor]
		public Workflow(Guid workflowId, string workflowname)
			: this(workflowname)
		{
			// don't generate a new GUID when deserializing the workflow
			WorkflowId = workflowId;
		}

		internal KeyValuePair<string, Node> GetFirstNode()
		{
			return Nodes.First();
		}

		public static Workflow DeserializeWorkflow(string workflowJson)
		{
			if (string.IsNullOrWhiteSpace(workflowJson))
			{
				throw new ArgumentException("Cannot create workflow from empty JSON");
			}
			// deserialize arbitrary "ITrackable" concrete type http://www.newtonsoft.com/json/help/html/SerializeTypeNameHandling.htm
			var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
			return JsonConvert.DeserializeObject<Workflow>(workflowJson, settings);
		}

		/// <summary>
		/// Restores from WSOD.Web.ObjectDataStore
		/// </summary>
		/// <param name="workflowId">Key that references a specific workflow</param>
		/// <param name="storeContext">Key that references the context the workflow was saved in</param>
		/// <returns>A workflow from the ObjectStore.</returns>
		/// <exception cref="ArgumentException">Invalid ID, No ObjectStore context</exception>
		public static Workflow RetrieveWorkflowFromObjectStore(Guid workflowId, string storeContext)
		{
			var wf = wfr.GetAll().First();
			return wf;
			throw new NotImplementedException("need to re-implement");
			//if (workflowId == Guid.Empty)
			//{
			//	throw new ArgumentException("Cannot restore from ObjectStore with an invalid Workflow ID");
			//}

			//if (string.IsNullOrWhiteSpace(storeContext))
			//{
			//	throw new ArgumentException("Cannot restore from ObjectStore with an empty store context");
			//}

			//// HACK
			//Testing.MockRequest(@"\internal");
			//// retrieve it from ObjectStore
			//var retrieveRequest = new MdObjectStoreServiceRequest(User.Current);
			//if (!retrieveRequest.RetrieveObjectStoreInputs(TestStoreCategoryKey, storeContext))
			//{
			//	throw new Exception("Could not retrieve workflow from ObjectStore");
			//}
			//var found = retrieveRequest.Inputs.FirstOrDefault(k => k.Key == workflowId.ToString());
			//if (string.IsNullOrWhiteSpace(found.Key))
			//{
			//	throw new Exception("Could not retrieve workflow from ObjectStore");
			//}
			//var serializedWorkfow = ((MOD.Common.Remoting.ByteString)found.Value).String;
			//if (string.IsNullOrWhiteSpace(serializedWorkfow))
			//{
			//	throw new Exception("Workflow [" + workflowId + "] with context '" + storeContext + "' was not found in the store.");
			//}

			//return DeserializeWorkflow(serializedWorkfow);
		}

		internal void RemoveItemFromWorkflow(string trackableId)
		{
			// delete the trackable
			return;
			
		}

		public void AddNode(string stateName, string fromState = null)
		{
			Nodes.Add(stateName, new Node(stateName));
			if (!string.IsNullOrWhiteSpace(fromState))
			{
				AddValidStateMovement(fromState, stateName);
			}
		}

		public void AddTrackableToStart(Trackable item)
		{
			throw new NotImplementedException();
			//item.Location.Add(this.WorkflowName, Nodes.First().Key);
		}
	
		public void MoveTrackable(Trackable item, string targetNodeName)
		{
			MoveToNode(item, targetNodeName, null);
		}
		private void MoveToNode(Trackable item, string targetNodeName, IUser user = null, string comment = "Moved Item")
		{
			throw new NotImplementedException();
			//Movement move;
			//if (IsMoveValid(item.Location[this.WorkflowName], targetNodeName, out move))
			//{
			//	item.Location.Remove(this.WorkflowName);
			//	item.Location.Add(this.WorkflowName, targetNodeName);
			//	item.MoveHistory.Add(new ExecutedMove(move) { ExecutionTime = DateTime.Now });
			//	return;
			//}
				
			//throw new WorkFlowException("move is not valid");
		}
		
		public void CopyToNode(Trackable item, string fromState, string toState, IUser copyUser, string comment = "Copied Item")
		{
			throw new NotImplementedException();
			//Movement move;
			//if (!IsMoveValid(fromState, toState, out move, copyUser))
			//{
			//	var msg = string.Format("{0} cannot copy {1} from {2} to {3}", copyUser, item, fromState, toState);
			//	throw new WorkFlowException(msg);
			//}
			//item.Location.Remove(this.WorkflowName);
			//item.Location.Add(this.WorkflowName, toState);
			//item.MoveHistory.Add(new ExecutedMove(move) { ExecutionTime = DateTime.Now });
		}

		public bool IsMoveValid(string from, string to, out Movement move, IUser user=null)
		{
			move = null;

			var mv = path.Where(m => m.To == to && m.From == from);
			if(mv.Count() > 0)
			{
				move = mv.First();
				return true;
			}
			return false;
			
		}

		internal  string FindNextNodeName(string nodeName)
		{
			
			var nextNodeName = this.path.Find(m => m.From == nodeName).To;
			if (nextNodeName == null || nextNodeName == String.Empty)
				throw new WorkFlowException("No next Node found");
			return nextNodeName;
			
		}

		
		public List<string> GetNodeNames()
		{
			return Nodes.Keys.ToList();
		}
		
		public string SerializeToJsonString()
		{
			// serialize arbitrary "ITrackable" concrete type http://www.newtonsoft.com/json/help/html/SerializeTypeNameHandling.htm
			var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
			return JsonConvert.SerializeObject(this, Formatting.None, settings);
		}

		/// <summary>
		/// Save to WSOD.Web.ObjectDataStore.
		/// </summary>
		/// <param name="storeContext">Key that references the context to save the workflow in</param>
		/// <returns>True if successful, False otherwise</returns>
		/// <exception cref="ArgumentException">No ObjectStore context</exception>
		public bool SaveToObjectStore(string storeContext)
		{
			wfr = new WorkflowRepository();
			wfr.Add(this);
			return true;
			throw new NotImplementedException("need to re-implement");
			//if (string.IsNullOrWhiteSpace(storeContext))
			//{
			//	throw new ArgumentException("Cannot persist to ObjectStore with an empty store context");
			//}

			//// HACK
			//Testing.MockRequest(@"\internal");
			//// store it into ObjectStore
			//var storeRequest = new MdObjectStoreServiceRequest(User.Current);
			//storeRequest.Inputs.Add(new KeyValuePair<string, object>(WorkflowId.ToString(), SerializeToJsonString()));
			//if (!storeRequest.StoreObjectStoreInputs(TestStoreCategoryKey, storeContext))
			//{
			//	throw new Exception("Could not persist workflow to ObjectStore");
			//}

			//return true;
		}

		//public Trackable FindTrackableById(string trackableId)
		//{
			
		//	foreach(KeyValuePair<string, Node> kvp in Nodes)
		//	{
		//		IEnumerable<Trackable> r = kvp.Value.Trackables.Where(t => t.TrackableId == trackableId);
		//		if (r.Count() > 0)
		//			return r.First();
		//	}
		//	return null;
			
			
		//}

		

		public void AddValidStateMovement(string from, string to, User moveUser = null)
		{
			if (!Nodes.ContainsKey(from))
			{
				throw new WorkFlowException("can't move from " + from);
			}

			if (!Nodes.ContainsKey(to))
			{
				throw new WorkFlowException("can't move to " + to);
			}

			// check if movement already exists
			Movement move = path.FirstOrDefault(m => m.From == from && m.To == to);
			if (move == null)
			{
				move = new Movement { From = from, To = to };
				path.Add(move);
			}

			// add move user
			if (moveUser != null)
			{
				move.ApproveUsers.Add(moveUser);
			}
		}

		//		public void AddValidStateRemoval(string from, IUser removeUser)
		//		{
		//			if (!Steps.ContainsKey(from))
		//			{
		//				throw new WorkFlowException("can't move from " + from);
		//			}
		//
		//			// should check if movement already exists
		//			var move = new Movement { From = from };
		//			move.ApproveUsers.Add(removeUser);
		//			path.Add(move);
		//		}
	}

}

