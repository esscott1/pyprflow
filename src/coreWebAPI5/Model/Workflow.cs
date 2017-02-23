using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace coreWebAPI5.Model
{
	public class Workflow
	{
		private static string TestStoreCategoryKey = "WorkFlow_ProofOfConcept";
		public Guid WorkflowId { get; private set; }
		public string Key { get; set; }
		public string WorkflowName { get; private set; }
		[JsonProperty]
		private Dictionary<string, Node> Nodes;
		//[JsonProperty]
		//private Dictionary<string, List<ITrackable>> Steps; // name is the key... needs to change to different
		[JsonProperty]
		private List<TrackingComment> trackingComments;
		[JsonProperty]
		private List<Movement> moves;

		private static IWorkflowRepository wfr { get; set; }
		public Workflow(string workflowName)
		{
			if (WorkflowId == Guid.Empty)
			{
				WorkflowId = Guid.NewGuid();
			}
			//Steps = new Dictionary<string, List<ITrackable>>();
			Nodes = new Dictionary<string, Node>();
			trackingComments = new List<TrackingComment>();
			moves = new List<Movement>();
			if (workflowName == "_blank")
			{
				Key="_blankKey";
				Trackable td = new Trackable("doc1") { TrackableId = "doc1" };
				Trackable td2 = new Trackable("doc2") { TrackableId = "doc2" };
				List<Trackable> l = new List<Trackable>();
				l.Add(td);l.Add(td2); 
				//Steps.Add("Step1", l);
				//Steps.Add("Step2", new List<ITrackable>());
				//Steps.Add("Step3", new List<ITrackable>());
				//Steps.Add("Step4", new List<ITrackable>());
				moves.Add(new Movement() { From = "Step1", To = "Step2" });
				moves.Add(new Movement() { From = "Step2", To = "Step3" });
				moves.Add(new Movement() { From = "Step3", To = "Step4" });
				Nodes.Add("Step1", new Node("Step1") { Trackables = l });
				foreach(Trackable t in l)
				{
					t.NodeNamesIn.Add("Step1");
				}
				Nodes.Add("Step2", new Node("Step2") );
			}
			WorkflowName = workflowName;
			
		}

		internal IEnumerable<string> FindAvailableNodes(string trackableId)
		{
			string nodeName = GetNodeNameItemIsIn(trackableId);
			IEnumerable<Movement> listOmoves = moves.Where(m => m.From == nodeName);
			IEnumerable<string> toNodes = listOmoves.Select(o => o.To);
			return toNodes;
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
			foreach(KeyValuePair<string, Node> kvp in Nodes)
			{
				var trackables = kvp.Value.Trackables.FindAll(t => t.TrackableId == trackableId);
				if (trackables.Count > 0)
				{
					kvp.Value.Trackables.RemoveAll(t => t.TrackableId == trackableId);
				}
			}
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
			Nodes.First().Value.Trackables.Add(item);
			item.NodeNamesIn.Add(Nodes.First().Key);
		}
		public void AddTrackableToState(Trackable item, string stateName, IUser moveUser, string comment = "Added Item")
		{
			Nodes[stateName].Trackables.Add(item);

			// log comment
			if (comment.Equals("Added Item"))
			{
				comment = string.Format("{0} to {1}", comment, stateName);
			}
			//TrackComment(item.TrackingGuid, comment, moveUser);
		}
		public void MoveToNode(string trackableId, string toState, IUser user = null, string comment = "Moved Item")
		{
			try
			{
				var trackable = this.GetTrackableById(trackableId);

				var currentNode = this.GetNodeNameItemIsIn(trackable).First();
				if (IsMoveValid(currentNode, toState))
				{
					Nodes[currentNode].Trackables.Remove(trackable);
					Nodes[toState].Trackables.Add(trackable);
					trackable.NodeNamesIn.Add(toState);
				}
				else
					throw new WorkFlowException("invalid move");

			}
			catch(Exception ex)
			{
				throw new WorkFlowException(ex.Message);
			
			}

		}
		//public void MoveToNode(Trackable item, string fromState, string toState, IUser moveUser, string comment = "Moved Item")
		//{
		//	if (!IsMoveValid(fromState, toState, moveUser))
		//	{
		//		var msg = string.Format("{0} cannot move {1} from {2} to {3}", moveUser, item, fromState, toState);
		//		throw new WorkFlowException(msg);
		//	}

		//	Nodes[toState].Trackables.Add(item);
		//	RemoveFromState(item, fromState, moveUser, toState);

		//	// log comment
		//	if (comment.Equals("Moved Item"))
		//	{
		//		comment = string.Format("{0} from {1} to {2}", comment, fromState, toState);
		//	}
		////	TrackComment(item.TrackingGuid, comment, moveUser);
		//}

		public void CopyToNode(Trackable item, string fromState, string toState, IUser copyUser, string comment = "Copied Item")
		{
			if (!IsMoveValid(fromState, toState, copyUser))
			{
				var msg = string.Format("{0} cannot copy {1} from {2} to {3}", copyUser, item, fromState, toState);
				throw new WorkFlowException(msg);
			}

			Nodes[toState].Trackables.Add(item);

			// log comment
			if (comment.Equals("Copied Item"))
			{
				comment = string.Format("{0} from {1} to {2}", comment, fromState, toState);
			}
			//TrackComment(item.TrackingGuid, comment, copyUser);
		}

		public bool IsMoveValid(string from, string to, IUser user=null)
		{
			if (!moves.Any())
			{
				return true;
			}

			var validMoves = moves.Where(m => m.From == from && m.To == to);
			// can we move?
			if (!validMoves.Any())
			{
				return false;
			}

			// do we have any approvers if not, just approve?
			var numApprovers = validMoves.Sum(u => u.ApproveUsers.Count);
			if (numApprovers == 0)
			{
				return true;
			}

			// do we have the correct approver?
			if (!validMoves.Any(u => u.ApproveUsers.Contains(user)))
			{
				return false;
			}

			return true;
		}

		public void RemoveFromState(Trackable item, string stateName, IUser removeUser, string comment = "Removed Item")
		{
			Nodes[stateName].Trackables.Remove(item);

			// log comment
			if (comment.Equals("Removed Item"))
			{
				comment = string.Format("{0} from {1}", comment, stateName);
			}
			//TrackComment(item.TrackingGuid, comment, removeUser);
		}
		internal  string FindNextNodeName(string nodeName)
		{
			
			var nextNodeName = this.moves.Find(m => m.From == nodeName).To;
			if (nextNodeName == null || nextNodeName == String.Empty)
				throw new WorkFlowException("No next Node found");
			return nextNodeName;
			
		}

		
		public List<string> GetNodeNames()
		{
			return Nodes.Keys.ToList();
		}

		public string GetNodeNameItemIsIn(string itemId)
		{
			foreach(KeyValuePair<string, Node> kvp in Nodes)
			{
				var r = kvp.Value.Trackables.Where(t => t.TrackableId == itemId);
				if (r.Count() > 0)
					return kvp.Key;
			}
			return String.Empty;


		}
		public IEnumerable<string> GetNodeNameItemIsIn(Trackable item)
		{
	
			return Nodes.Where(n => n.Value.Trackables.Contains(item)).Select(k => k.Key);
			
		}

		public IEnumerable<Trackable> GetItemsInNode(string stateName)
		{
			Node node = Nodes[stateName];
			return node.Trackables;
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

		private void TrackComment(Guid docId, string theComment, IUser user)
		{
			trackingComments.Add(new TrackingComment(theComment, user, docId));
		}

		public IEnumerable<TrackingComment> GetTrackingByDocument(Guid docId)
		{
			return trackingComments.Where(t => t.DocId == docId).OrderBy(o => o.Time);
		}

		public Trackable GetTrackableById(string docId)
		{
			
			foreach(KeyValuePair<string, Node> kvp in Nodes)
			{
				IEnumerable<Trackable> r = kvp.Value.Trackables.Where(t => t.TrackableId == docId);
				if (r.Count() > 0)
					return r.First();
			}
			return null;
			
			
		}

		public IEnumerable<TrackingComment> GetTrackingByUser(IUser user)
		{
			return trackingComments.Where(t => t.User == user).OrderBy(o => o.Time);
		}

		public void AddValidStateMovement(string from, string to, IUser moveUser = null)
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
			Movement move = moves.FirstOrDefault(m => m.From == from && m.To == to);
			if (move == null)
			{
				move = new Movement { From = from, To = to };
				moves.Add(move);
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
		//			moves.Add(move);
		//		}
	}

}

