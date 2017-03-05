﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Dependency­Injection;

namespace workflow.Model
{
	public class Trackable : WorkflowItem
	{
		public string TrackableId { get; set; }
		public string Key { get; set; }
		public string TrackingName { get; set; }
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
			TrackingName = trackableName;
			TrackableId = trackableName;
			Key = trackableName;
			

		}

		internal void Demo(string workflowId, string nodeId)
		{
			this.Locations.Add(new Location()
			{
				WorkflowId = workflowId,
				NodeId = nodeId
			});
		}

		public Trackable()
		{
			Locations = new List<Location>();
			//Trackable td = new Trackable("doc1")
			//{ TrackableId2 = "doc1" };
			//td.Locations.Add(new Location()
			//{ WorkflowGuid = "_blank", NodeId = "Step1" });
			//Trackable td2 = new Trackable("doc2") { TrackableId2 = "doc2" };
			//td.Locations.Add(new Location()
			//{ WorkflowGuid = "_blank", NodeId = "Step2" });
		}

		
	}
	public class Location
	{
		public string WorkflowId { get; set; }
		public string NodeId { get; set; }
	}
}