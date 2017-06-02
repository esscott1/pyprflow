using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyprflow.Model
{
	public class Transaction : BaseWorkflowItem 
	{
		
		public string TrackableName { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public TransactionType type { get; set; }
		public string Comment { get; set; }
		public string CurrentNodeId { get; set; }
		
		public string NewNodeId { get; set; }
		public DateTime TransActionTime { get; internal set; }
		[JsonRequired]
		public User Submitter { get; set; }
		public User AssignedTo { get; set; }
		public string WorkflowName { get; set; }
		public Transaction()
		{
			TransActionTime = DateTime.Now;
		}
		public void Clean()
		{
			if(this.type== TransactionType.move || this.type==TransactionType.copy)
			{
				this.AssignedTo = null;
				//this.Comment = null;
			}
			if(this.type == TransactionType.comment) {
				this.NewNodeId = null;
				this.AssignedTo = null;
			}

			if (this.type == TransactionType.assignment)
			{
				this.NewNodeId = null;
				//this.Comment = null;
			}
		}
		public bool Execute(IWorkflowRepository repository, out int statuscode, out string statusmessage)
		{
			this.Clean();
			if (!this.IsUniqueTransactionId(repository, out statuscode, out statusmessage))
				return false;
			if (!this.IsValid(repository, out statuscode, out statusmessage))
				return false;
			Console.WriteLine("executing the transaction");
			statuscode = 0;
			statusmessage = "success";
			var workflow = repository.Find<Workflow>(WorkflowName);

			// should the transaction object execute
			//trans.Execute();
			if (this.type == TransactionType.copy || this.type == TransactionType.move)
			{
				if (!workflow.IsMoveValid(this, repository))
				{
					statuscode = 400;
					statusmessage = "move is not valid per workflow rules";
					return false;
				}
				Console.WriteLine("passed IsValid validation");
				
			}
			repository.Add(this);Console.WriteLine("added now going to track");

			repository.Track(this);
			return true;
		}
		private bool IsValid(IWorkflowRepository repository, out int statuscode, out string statusmessage)
		{
			statuscode = 0;
			statusmessage = "success";
			Console.WriteLine("checking for valid transaction");
			try
			{
				Console.WriteLine("type of transaction is {0}",this.type.ToString());
				if (this.type == TransactionType.move || this.type == TransactionType.copy)
				{
					return IsValidMoveCopy(repository, out statuscode, out statusmessage);
				}
				if (this.type == TransactionType.assignment)
				{
					return IsValidAssignment(out statuscode, out statusmessage);
				}
				if (this.type == TransactionType.comment)
				{
					Console.WriteLine("in the comment if statement");
					bool result = IsValidComment(out statuscode, out statusmessage);
					Console.WriteLine("is valid comment result is {0}", result);
					return result;
				}
			} 
			catch(Exception ex)
			{
				Console.WriteLine("an error occured {0}",ex.Message);
			}
				statuscode = 400;
				statusmessage = "not a valid type of transaction";
			
			return false;
				
		}

		private bool IsValidComment(out int statuscode, out string statusmessage)
		{
			statuscode = 0;
			statusmessage = "success";
		
			bool result = false;
			
				result = (!string.IsNullOrEmpty(this.CurrentNodeId) &&
					!string.IsNullOrEmpty(this.Submitter.Email) &&
					!string.IsNullOrEmpty(this.TrackableName) &&
					!string.IsNullOrEmpty(this.WorkflowName) &&
					!string.IsNullOrEmpty(this.Comment));
			if(!result)
			{
				statuscode = 400;
				statusmessage = string.Format("missing a required value, you sent {0}, {1},{2}, {3}, {4}",
					this.CurrentNodeId, this.Submitter.Email, this.TrackableName, this.WorkflowName, this.Comment);
			}
			return result;
		}

		private bool IsValidAssignment(out int statuscode, out string statusmessage)
		{
			statuscode = 0;
			statusmessage = "success";

			bool result = (!string.IsNullOrEmpty(this.CurrentNodeId) &&
				!string.IsNullOrEmpty(this.Submitter.Email) &&
				!string.IsNullOrEmpty(this.TrackableName) &&
				!string.IsNullOrEmpty(this.WorkflowName));
			if(!result)
			{
				statuscode = 400;
				statusmessage = "missing a required value";
			}
			return result;
		}

		private bool IsUniqueTransactionId(IWorkflowRepository repository, out int statuscode, out string statusmessage)
		{
			statuscode = 0;
			statusmessage = string.Empty;
			if (repository.Find<Transaction>(this.Name) != null)
			{
				statuscode = 400;
				statusmessage = string.Format("The transactionId {0} already exists", this.Name);
				return false;
			}
			return true;


		}

		private bool IsValidMoveCopy(IWorkflowRepository repository, out int statuscode, out string statusmessage)
		{
			statuscode = 0;
			statusmessage = string.Empty;

			//if (!IsUniqueTransactionId(repository, out statuscode, out statusmessage))
			//	return false;
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
			// if previousNodeID is null then it's a new entry into the system
			if (this.CurrentNodeId != null)
			{
				Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dic =
					new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
				dic.Add("isactive", "true");
				dic.Add("nodeid", this.CurrentNodeId);
				dic.Add("trackableid", this.TrackableName);
				dic.Add("entityType", "trackables");
					Console.WriteLine("looking for trackables in {0} with trackableId = {1}", this.CurrentNodeId, this.TrackableName);
				Db.SearchRequest request = new Db.SearchRequest(dic);
				Db.SearchEngine se = new Db.SearchEngine(repository);

				var response = se.Search(request);
				//Console.WriteLine("found {0} number of {1} in node {2}", response.Count, this.TrackableName, this.CurrentNodeId);
				if (response.Count == 0)
				{
					statuscode = 400;
					statusmessage = string.Format("Trackable {0} is not in the previous node {1} like you say it is", this.TrackableName, this.CurrentNodeId);
					return false;
				}

			}
			return true;
		}

		public bool Equals(Transaction other)
		{
			if (this.NewNodeId == other.NewNodeId &&
				this.CurrentNodeId == other.CurrentNodeId)
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
		move,
		copy,
		assignment,
		comment
	}
}
