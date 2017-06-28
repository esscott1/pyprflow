using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyprflow.Workflow.Model
{
	public class WorkflowAction
	{
		public string WorkflowId { get; set; }
		public string TrackableId { get; set; }
		public string NodeId { get; set; }
		public string Comment { get; set; }
	}
}
