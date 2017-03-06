using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workflow.Model
{
    public class Orchestration : WorkflowItem
    {
		public string OrchestrationName { get; set; }
		public List<Movement> Moves { get; set; }

		public bool IsValid(Transaction transaction)
		{

			return true;
		}
    }
}
