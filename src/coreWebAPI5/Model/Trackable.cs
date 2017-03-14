using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Dependency­Injection;

namespace workflow.Model
{
	public class Trackable : BaseWorkflowItem
	{
		//public string TrackableName { get; set; }
		//public string TrackingName { get; set; }
		//public List<string> NodeNames { get; set; }
		/// <summary>
		/// Location is workflow and node 
		/// </summary>
		public List<Location> Locations { get; set; }
		
	//	public Location CurrentLocation { get; set; }
		//[JsonIgnore]
		//public List<ExecutedMove> MoveHistory { get; set; }
		public Trackable(string trackableName) : this()
		{
			Name = trackableName;
			//TrackingName = trackableName;
			//TrackableName = trackableName;
			

		}

		

		public Trackable()
		{
			Locations = new List<Location>();
		}

		
	}
	public class Location
	{
		public string WorkflowName { get; set; }
		public string NodeId { get; set; }
	}
}