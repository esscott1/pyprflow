using System.Collections.Generic;
using System.Runtime.Serialization;

namespace workflow.Model
{
	public class Node
	{
		public Node(string nodeName)
		{
			NodeName = nodeName;
			
		}
		public string NodeName { get; set; }
		public string NodeDescription { get; set; }
		public string NodeId { get; set; }
		public bool IsStart { get; set; }
		public bool IsEnd { get; set; }
	}

	
}