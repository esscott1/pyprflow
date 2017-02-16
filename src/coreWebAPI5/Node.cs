using System.Collections.Generic;

namespace coreWebAPI5.Model
{
	internal class Node
	{
		public Node(string nodeName)
		{
			NodeName = nodeName;

		}
		public List<ITrackable> Trackables { get; set; }
		public string NodeName { get; set; }
		public string NodeDescription { get; set; }
		public int NodeId { get; set; }
	}
}