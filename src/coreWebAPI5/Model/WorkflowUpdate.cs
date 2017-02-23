using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workflow.Model
{
	public class WorkflowUpdate
	{
		public string WorkflowId { get; set; }
		public string TrackableId { get; set; }
		public string NodeId { get; set; }
	}
}
