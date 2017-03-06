using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace workflow.Model
{
	public class Workflow : WorkflowItem
	{
		private static string TestStoreCategoryKey = "WorkFlow_ProofOfConcept";
		public Guid WorkflowGuid { get; private set; }
		public string Key { get; set; }
		public string WorkflowName { get; private set; }

		internal string StartingNodeName  {get;set;}
		internal string EndingNodeName { get; set; }

		[JsonProperty]
		internal Dictionary<string, Node> Nodes;
	
		[JsonProperty]
		internal Dictionary<string, Orchestration> Orchestrations;

		private static IWorkflowRepository Repository { get; set; }
		public Workflow(string workflowName) : this()
		{
			if (WorkflowGuid == Guid.Empty)
			{
				WorkflowGuid = Guid.NewGuid();
			}
		}

		public Workflow() {
			this.Orchestrations = new Dictionary<string, Orchestration>();
			this.Nodes = new Dictionary<string, Node>();
		}

		internal Workflow GetSample()
		{
			IUser user = new User() { Email = "Sample.User@somewhere.com" };
			List<User> users = new List<User> { new User() { Email = "Sample.User@somewhere.com" } };
			Model.Workflow w = new Model.Workflow("SampleWorkflow1");
			w.Key = "SampleWorkflow1";
			w.Orchestrations.Add("Orch1", new Orchestration()
			{
				OrchestrationName = "Orch1",
				Moves = new List<Movement>() {
					new Movement() { From = null, To = "SampleNode1" },
					new Movement() { From = "SampleNode1", To = "SampleNode2"},
					new Movement() { From = "SampleNode2", To = "SampleNode3"},
					new Movement() { From = "SampleNode3", To = "SampleNode4"}
					}
			});
			w.Orchestrations.Add(
				"Orch2", new Orchestration()
				{
					OrchestrationName = "Orch2",
					Moves = new List<Movement>() {
					new Movement() { From = null, To = "SampleNode1" },
					new Movement() { From = "SampleNode1", To = "SampleNode2"},
					new Movement() { From = "SampleNode2", To = "SampleNode3"},
					new Movement() { From = "SampleNode2", To = "SampleNode4"},
					new Movement() { From = "SampleNode2", To = "SampleNode1"},
					new Movement() { From = "SampleNode3", To = "SampleNode2"},
					new Movement() { From = "SampleNode3", To = "SampleNode4"}
					}
				});

			//w.Orchestrations.Add(new Movement() { From = null, To = "SampleNode1", ApproveUsers = users });
			//w.Orchestrations.Add(new Movement() { From = "SampleNode1", To = "SampleNode2", ApproveUsers = users });
			//w.Orchestrations.Add(new Movement() { From = "SampleNode2", To = "SampleNode3", ApproveUsers = users });
			//w.Orchestrations.Add(new Movement() { From = "SampleNode3", To = "SampleNode4", ApproveUsers = users });
			w.Nodes.Add("SampleNode1", new Node("SampleNode1") { IsStart = true });
			w.Nodes.Add("SampleNode2", new Node("SampleNode2"));
			w.Nodes.Add("SampleNode3", new Node("SampleNode3"));
			w.Nodes.Add("SampleNode4", new Node("SampleNode4") { IsEnd = true });
			return w;
			
		}

		private bool CanExitNode(string nodeName)
		{
			throw new NotImplementedException();
			
		}

		private bool CanEnterNode(string nodeName)
		{
			throw new NotImplementedException();	
			//foreach (Movement m in Orchestrations)
			//{
			//	if (m.To == nodeName)
			//		return true;
			//}
			//return false;

		}
		internal bool IsValid(out WorkflowValidationMessage message)
		{
			throw new NotImplementedException();
			//message = new WorkflowValidationMessage();
			//List<string> NodesWithoutProperPaths = new List<string>();
			//// all middle nodes can be moved into and out of 
			//foreach(KeyValuePair<string, Node> kvp in Nodes)
			//{
			//	if (kvp.Value.IsStart) { message.HasStart = true; }
			//	else {
			//		if (!CanEnterNode(kvp.Key))
			//			message.UnreachableNodeNames.Add(kvp.Key); 
			//		}
			//	if (kvp.Value.IsEnd) {	message.HasEnd = true; }
			//	else
			//	{
			//		if (!CanExitNode(kvp.Key))
			//			message.DeadEndNodeNames.Add(kvp.Key);
			//	}
			//}
			//if (message.Valid)
			//	return true;
			//return false;
		}


		[JsonConstructor]
		public Workflow(Guid workflowId, string workflowname)
			: this(workflowname)
		{
			// don't generate a new GUID when deserializing the workflow
			WorkflowGuid = workflowId;
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
		//public static Repository RetrieveWorkflowFromObjectStore(Guid workflowId, string storeContext)
		//{
		//	var wf = Repository.GetAll<Repository>();
		//	return wf;
		//	throw new NotImplementedException("need to re-implement");
		//	//if (workflowId == Guid.Empty)
		//	//{
		//	//	throw new ArgumentException("Cannot restore from ObjectStore with an invalid Repository ID");
		//	//}

		//	//if (string.IsNullOrWhiteSpace(storeContext))
		//	//{
		//	//	throw new ArgumentException("Cannot restore from ObjectStore with an empty store context");
		//	//}

		//	//// HACK
		//	//Testing.MockRequest(@"\internal");
		//	//// retrieve it from ObjectStore
		//	//var retrieveRequest = new MdObjectStoreServiceRequest(User.Current);
		//	//if (!retrieveRequest.RetrieveObjectStoreInputs(TestStoreCategoryKey, storeContext))
		//	//{
		//	//	throw new Exception("Could not retrieve workflow from ObjectStore");
		//	//}
		//	//var found = retrieveRequest.Inputs.FirstOrDefault(k => k.Key == workflowId.ToString());
		//	//if (string.IsNullOrWhiteSpace(found.Key))
		//	//{
		//	//	throw new Exception("Could not retrieve workflow from ObjectStore");
		//	//}
		//	//var serializedWorkfow = ((MOD.Common.Remoting.ByteString)found.Value).String;
		//	//if (string.IsNullOrWhiteSpace(serializedWorkfow))
		//	//{
		//	//	throw new Exception("Repository [" + workflowId + "] with context '" + storeContext + "' was not found in the store.");
		//	//}

		//	//return DeserializeWorkflow(serializedWorkfow);
		//}

		internal void RemoveItemFromWorkflow(string trackableId)
		{
			// delete the trackables
			return;
			
		}

		//public void AddNode(string stateName, string fromState = null)
		//{
		//	Nodes.Add(stateName, new Node(stateName));
		//	if (!string.IsNullOrWhiteSpace(fromState))
		//	{
		//		//AddValidStateMovement(fromState, stateName);
		//	}
		//}

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

		public bool IsMoveValid(Transaction transaction)
		{
			foreach (KeyValuePair<string, Orchestration> kvp in Orchestrations)
			{
				if (kvp.Value.IsValid(transaction))
					return true;
			}
			return false;
			
		}

		internal  string FindNextNodeName(string nodeName)
		{
			throw new NotImplementedException();
			//var nextNodeName = this.Orchestrations.Find(m => m.From == nodeName).To;
			//if (nextNodeName == null || nextNodeName == String.Empty)
			//	throw new WorkFlowException("No next Node found");
			//return nextNodeName;
			
		}

		
		public List<string> GetNodeNames()
		{
			return Nodes.Keys.ToList();
		}
		
		//public string SerializeToJsonString()
		//{
		//	// serialize arbitrary "ITrackable" concrete type http://www.newtonsoft.com/json/help/html/SerializeTypeNameHandling.htm
		//	var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
		//	return JsonConvert.SerializeObject(this, Formatting.None, settings);
		//}

		/// <summary>
		/// Save to WSOD.Web.ObjectDataStore.
		/// </summary>
		/// <param name="storeContext">Key that references the context to save the workflow in</param>
		/// <returns>True if successful, False otherwise</returns>
		/// <exception cref="ArgumentException">No ObjectStore context</exception>
		public bool SaveToObjectStore(string storeContext)
		{
			Repository = new WorkflowRepository();
			Repository.Add(this);
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
			//storeRequest.Inputs.Add(new KeyValuePair<string, object>(WorkflowGuid.ToString(), SerializeToJsonString()));
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
		//		IEnumerable<Trackable> r = kvp.Value.Trackables.Where(t => t.TrackableId2 == trackableId);
		//		if (r.Count() > 0)
		//			return r.First();
		//	}
		//	return null;
			
			
		//}

		public void AddValidStateMovement(string from, string to, User moveUser = null)
		{
			throw new NotImplementedException();
			//if (!Nodes.ContainsKey(from))
			//{
			//	throw new WorkFlowException("can't move from " + from);
			//}

			//if (!Nodes.ContainsKey(to))
			//{
			//	throw new WorkFlowException("can't move to " + to);
			//}

			//// check if movement already exists
			//Movement move = Orchestrations.FirstOrDefault(m => m.From == from && m.To == to);
			//if (move == null)
			//{
			//	move = new Movement { From = from, To = to };
			//	Orchestrations.Add(move);
			//}

			//// add move user
			//if (moveUser != null)
			//{
			//	move.ApproveUsers.Add(moveUser);
			//}
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
		//			Orchestrations.Add(move);
		//		}
	}

}

