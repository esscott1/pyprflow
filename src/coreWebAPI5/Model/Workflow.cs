using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace workflow.Model
{
	public class Workflow : BaseWorkflowItem
	{
		private static string TestStoreCategoryKey = "WorkFlow_ProofOfConcept";
		public Guid WorkflowGuid { get; private set; }
		//public string WFKey { get; set; }
		public string WorkflowName { get; private set; }

		internal string StartingNodeName  {get;set;}
		internal string EndingNodeName { get; set; }

		[JsonProperty]
		internal Dictionary<string, Node> Nodes;
	
		[JsonProperty]
		internal Dictionary<string, Orchestration> Orchestrations;
		/// <summary>
		/// true (default): a trackable can not move between orchestrations within a given workflow.  false: it can
		/// </summary>
		public bool OrchestrationAffinity { get; set; } = true;

		private static IWorkflowRepository Repository { get; set; }
		public Workflow(string workflowName) : this()
		{
			this.Name = workflowName;
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
			//w.WFKey = "SampleWorkflow1";
			w.Orchestrations.Add("Orch1", new Orchestration()
			{
				OrchestrationName = "Orch1",
				Moves = new List<Movement>() {
					new Movement() { From = null, To = "SampleNode1",
					 Rule = new Rule() { AccessList = new List<User>() {
						 new User() { Email = "ericscott411@gmail.com" } } } },
	
					new Movement() { From = "SampleNode1", To = "SampleNode2",
						Rule = new Rule() { AccessList = new List<User>() {
						 new User() { Email = "ericscott411@gmail.com" } } } },

					new Movement() { From = "SampleNode2", To = "SampleNode3",
						Rule = new Rule() { AccessList = new List<User>() {
						 new User() { Email = "ericscott411@gmail.com" } } } },
					
					new Movement() { From = "SampleNode3", To = "SampleNode4",
						Rule = new Rule() { AccessList = new List<User>() {
						 new User() { Email = "ericscott411@gmail.com" } } } },
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
			w.OrchestrationAffinity = false;
			w.Nodes.Add("SampleNode1", new Node("SampleNode1") { IsStart = true });
			w.Nodes.Add("SampleNode2", new Node("SampleNode2"));
			w.Nodes.Add("SampleNode3", new Node("SampleNode3"));
			w.Nodes.Add("SampleNode4", new Node("SampleNode4") { IsEnd = true });
			return w;
			
		}

		private bool CanExitNode(string nodeName)
		{
			return true;
			throw new NotImplementedException();
			
		}

		private bool CanEnterNode(string nodeName)
		{
			//throw new NotImplementedException();
			foreach (KeyValuePair<string, Orchestration> kvp in Orchestrations)
			{
				foreach (Movement m in kvp.Value.Moves)
				{
					if (m.To == nodeName)
						return true;
				}
			}
			return false;
		}
		
		internal bool IsValid(out WorkflowValidationMessage message)
		{
			//throw new NotImplementedException();
			message = new WorkflowValidationMessage();
			List<string> NodesWithoutProperPaths = new List<string>();
			// checking Node Validation
			// all middle nodes can be moved into and out of 
			foreach (KeyValuePair<string, Node> kvp in Nodes)
			{
				if (kvp.Value.IsStart) { message.HasStart = true; }
				else
				{
					if (!CanEnterNode(kvp.Key))
						message.UnreachableNodeNames.Add(kvp.Key);
				}
				if (kvp.Value.IsEnd) { message.HasEnd = true; }
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

		public bool IsMoveValid(Transaction transaction, IWorkflowRepository repository)
		{
			foreach (KeyValuePair<string, Orchestration> kvp in Orchestrations)
			{
				if (kvp.Value.IsValid(transaction))
					return true;
			}
			return false;
		}

		


		/// <summary>
		/// Save to WSOD.Web.ObjectDataStore.
		/// </summary>
		/// <param name="storeContext">WFKey that references the context to save the workflow in</param>
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

		
	}

}

