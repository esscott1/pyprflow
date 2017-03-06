using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workflow.Model
{
    public class Rule : WorkflowItem
    {
		// those that can execute this rule
		public List<User> AccessList { get; set; }

    }
}
