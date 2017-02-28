using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workflow.Model
{
	public class Transaction
	{
		public string  Key { get; set; }
		public string TrackableId { get; set; }
		public TransactionType type { get; set; }
		public string Comment { get; set; }
		public string PreviousNodeId { get; set; }
		public string NewNodeId { get; set; }
		public DateTime TransActionTime { get; internal set; }
		public User Submitter { get; set; }
		public string WorkflowId { get; set; }
		public Transaction()
		{
			TransActionTime = DateTime.Now;
		}
	}


	 
	public enum TransactionType
	{
		Move = 1,
		Copy = 2
	}
}
