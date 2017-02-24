﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workflow.Model
{
	public class Trackable
	{
		//	public int ItemId { get; set; }
		public string TrackableId { get; set; }
		public string TrackingName { get; set; }
		public List<string> NodeNamesIn { get; set; }

		public List<ExecutedMove> MoveHistory { get; set; }
		public Trackable(string name)
		{
			NodeNamesIn = new List<string>();
			TrackingName = name;
			TrackableId = name;
			MoveHistory = new List<ExecutedMove>();
		}
		

	}
}
