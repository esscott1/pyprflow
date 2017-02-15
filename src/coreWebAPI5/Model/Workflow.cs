using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace coreWebAPI5.Model
{
	public class Workflow
	{
		private static string TestStoreCategoryKey = "MkDWorkFlow_ProofOfConcept";
		public Guid WorkflowId { get; private set; }
		public string Key { get; set; }
		public string WorkflowName { get; private set; }
		[JsonProperty]
		private Dictionary<string, List<ITrackable>> Steps;
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
			Steps = new Dictionary<string, List<ITrackable>>();
			trackingComments = new List<TrackingComment>();
			moves = new List<Movement>();
			if (workflowName == "_blank")
			{
				Key="_blankKey";
				Steps.Add("Step1", new List<ITrackable>());
				Steps.Add("Step2", new List<ITrackable>());
				Steps.Add("Step3", new List<ITrackable>());
				Steps.Add("Step4", new List<ITrackable>());
				moves.Add(new Movement() { From = "Step1", To = "Step2" });
				moves.Add(new Movement() { From = "Step2", To = "Step3" });
				moves.Add(new Movement() { From = "Step3", To = "Step4" });


			}

			WorkflowName = workflowName;
			
		}

		[JsonConstructor]
		public Workflow(Guid workflowId, string workflowname)
			: this(workflowname)
		{
			// don't generate a new GUID when deserializing the workflow
			WorkflowId = workflowId;
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

		public void AddState(string stateName, string fromState = null)
		{
			Steps.Add(stateName, new List<ITrackable>());
			if (!string.IsNullOrWhiteSpace(fromState))
			{
				AddValidStateMovement(fromState, stateName);
			}
		}

		public void AddTrackableToStart(ITrackable item)
		{
			Steps.First().Value.Add(item);

		}
		public void AddTrackableToState(ITrackable item, string stateName, IUser moveUser, string comment = "Added Item")
		{
			Steps[stateName].Add(item);

			// log comment
			if (comment.Equals("Added Item"))
			{
				comment = string.Format("{0} to {1}", comment, stateName);
			}
			TrackComment(item.TrackingGuid, comment, moveUser);
		}

		public void MoveToState(ITrackable item, string fromState, string toState, IUser moveUser, string comment = "Moved Item")
		{
			if (!IsMoveValid(fromState, toState, moveUser))
			{
				var msg = string.Format("{0} cannot move {1} from {2} to {3}", moveUser, item, fromState, toState);
				throw new WorkFlowException(msg);
			}

			Steps[toState].Add(item);
			RemoveFromState(item, fromState, moveUser, toState);

			// log comment
			if (comment.Equals("Moved Item"))
			{
				comment = string.Format("{0} from {1} to {2}", comment, fromState, toState);
			}
			TrackComment(item.TrackingGuid, comment, moveUser);
		}

		public void CopyToState(ITrackable item, string fromState, string toState, IUser copyUser, string comment = "Copied Item")
		{
			if (!IsMoveValid(fromState, toState, copyUser))
			{
				var msg = string.Format("{0} cannot copy {1} from {2} to {3}", copyUser, item, fromState, toState);
				throw new WorkFlowException(msg);
			}

			Steps[toState].Add(item);

			// log comment
			if (comment.Equals("Copied Item"))
			{
				comment = string.Format("{0} from {1} to {2}", comment, fromState, toState);
			}
			TrackComment(item.TrackingGuid, comment, copyUser);
		}

		public bool IsMoveValid(string from, string to, IUser user)
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

		public void RemoveFromState(ITrackable item, string stateName, IUser removeUser, string comment = "Removed Item")
		{
			Steps[stateName].Remove(item);

			// log comment
			if (comment.Equals("Removed Item"))
			{
				comment = string.Format("{0} from {1}", comment, stateName);
			}
			TrackComment(item.TrackingGuid, comment, removeUser);
		}

		public List<string> GetStates()
		{
			return Steps.Keys.ToList();
		}

		public IEnumerable<string> GetStatesItemIsIn(ITrackable item)
		{
			return Steps.Where(s => s.Value.Contains(item)).Select(k => k.Key);
		}

		public IEnumerable<ITrackable> GetItemsInState(string stateName)
		{
			return Steps[stateName];
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

		public IEnumerable<TrackingComment> GetTrackingByUser(IUser user)
		{
			return trackingComments.Where(t => t.User == user).OrderBy(o => o.Time);
		}

		public void AddValidStateMovement(string from, string to, IUser moveUser = null)
		{
			if (!Steps.ContainsKey(from))
			{
				throw new WorkFlowException("can't move from " + from);
			}

			if (!Steps.ContainsKey(to))
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

