using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Dependency­Injection;

namespace pyprflow.Workflow.Model
{
	public class Trackable : BaseWorkflowItem
	{
		
		/// <summary>
		/// Location is workflow and node 
		/// </summary>
		//public List<Location> Locations { get; set; }
		
	
		public Trackable(string trackableName) : this()
		{
			Name = trackableName;
		}

		

		public Trackable()
		{
		
		}

		
	}
	public class Location
	{
		public string WorkflowName { get; set; }
		public string NodeId { get; set; }
	}
}