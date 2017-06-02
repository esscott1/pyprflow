﻿using System;
using Newtonsoft.Json;

namespace pyprflow.Model
{
	[JsonConverter(typeof(Trackable))]
	public interface ITrackable
	{
		//int ItemId { get; set; }
	//	string TrackingGuid { get; set; }
		string TrackingName { get; set; }
	}
}
