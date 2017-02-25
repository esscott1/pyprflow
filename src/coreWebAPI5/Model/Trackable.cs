using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workflow.Model
{
	public class Trackable
	{
		public string TrackableId { get; set; }
		public string Key { get; set; }
		public string TrackingName { get; set; }
		public List<string> NodeNamesIn { get; set; }

		/// <summary>
		/// Location is workflow and node 
		/// </summary>
		public Dictionary<string, string> Location { get; set; }
		public List<ExecutedMove> MoveHistory { get; set; }
		public Trackable(string name, string workflowId)
		{
			NodeNamesIn = new List<string>();
			TrackingName = name;
			TrackableId = name;
			Key = name;
			MoveHistory = new List<ExecutedMove>();
			Location = new Dictionary<string, string>();
			//Location.Add(workflowId, "stuff");
		}
		

	}
}
