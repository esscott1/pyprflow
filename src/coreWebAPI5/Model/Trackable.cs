using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coreWebAPI5.Model
{
	public class Trackable
	{
		//	public int ItemId { get; set; }
		public string TrackableId { get; set; }
		public string TrackingName { get; set; }
		public List<string> NodeNamesIn { get; set; }
		public Trackable(string name)
		{
			NodeNamesIn = new List<string>();
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
