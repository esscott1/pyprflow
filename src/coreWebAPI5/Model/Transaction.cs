using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workflow.Model
{
	public class Transaction : BaseWorkflowItem 
	{
		
		public string TrackableName { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public TransactionType type { get; set; }
		public string Comment { get; set; }
		public string PreviousNodeId { get; set; }
		public string NewNodeId { get; set; }
		public DateTime TransActionTime { get; internal set; }
		public User Submitter { get; set; }
		public string WorkflowName { get; set; }
		public Transaction()
		{
			TransActionTime = DateTime.Now;
		}

		public bool IsValid(IWorkflowRepository repository, out int statuscode, out string statusmessage)
		{
			statuscode = 0;
			statusmessage = string.Empty;
			if (repository.Find<Transaction>(this.Name) != null)
			{
				statuscode = 400;
				statusmessage = string.Format("The transactionId {0} already exists", this.Name);
				return false;
			}
		//	Console.WriteLine("is unique transactionId");
			if (repository.Find<Trackable>(this.TrackableName) == null)
			{
				statuscode = 400;
				statusmessage = string.Format("The trackable {0} does not exists", this.TrackableName);
				return false;
			}
			//Console.WriteLine("the trackable exists in the system");
			var workflow = repository.Find<Workflow>(this.WorkflowName);
			if (workflow == null)
			{
				statuscode = 400;
				statusmessage = string.Format("The workflowId {0} does not exists", this.WorkflowName);
				return false;
			}
			//Console.WriteLine("the workflow exists in the system");
			Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dic =
				new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
			dic.Add("isactive", "true");
			dic.Add( "nodeid", this.PreviousNodeId);
			dic.Add("trackableid", this.TrackableName);
			dic.Add("entityType", "trackables");
		//	Console.WriteLine("looking for trackables in {0} with trackableId = {1}", this.PreviousNodeId, this.TrackableName);
			Db.SearchRequest request = new Db.SearchRequest(dic);
			Db.SearchEngine se = new Db.SearchEngine(repository);

			var response = se.Search(request);
			//Console.WriteLine("found {0} number of {1} in node {2}", response.Count, this.TrackableName, this.PreviousNodeId);
			if (response.Count == 0)
			{
				statuscode = 400;
				statusmessage = string.Format("Trackable {0} is not in the previous node {1} like you say it is", this.TrackableName, this.PreviousNodeId);
				return false;
			}
			return true;
		}


		public bool Equals(Transaction other)
		{
			if (this.NewNodeId == other.NewNodeId &&
				this.PreviousNodeId == other.PreviousNodeId)
			{
				return true;
			}
			return false;
		}
	}

	internal class TranactionComparer : IEqualityComparer<Transaction>
	{
		public bool Equals(Transaction t1, Transaction t2)
		{
			return t1.Equals(t2);
		}
		public int GetHashCode(Transaction obj)
		{
			return obj.NewNodeId.GetHashCode();
		}

	
	}


	 
	public enum TransactionType
	{
		Move,
		Copy
	}
}
