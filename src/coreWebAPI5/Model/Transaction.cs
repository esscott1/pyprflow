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
