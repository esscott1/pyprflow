using System.Collections.Generic;
using System.Runtime.Serialization;

namespace coreWebAPI5.Model
{
	internal class Node
	{
		public Node(string nodeName)
		{
			NodeName = nodeName;
			
			Trackables = new List<Trackable>();
		}
		public List<Trackable> Trackables { get; set; }
		public string NodeName { get; set; }
		public string NodeDescription { get; set; }
		public string NodeId { get; set; }
	}

	public class Trackable 
	{
		//	public int ItemId { get; set; }
		public string TrackableId { get; set; }
		public string TrackingName { get; set; }
		public Trackable(string name)
		{
			TrackingName = name;
			TrackableId = name;
		}
		//public Trackable(string trackingname, string trackableId)
		//{
		//	TrackingName = trackingname;
		//	TrackableId = trackableId;

		//}
	}
}