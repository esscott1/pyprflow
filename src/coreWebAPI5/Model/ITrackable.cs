using System;
using Newtonsoft.Json;

namespace coreWebAPI5.Model
{
	[JsonConverter(typeof(Trackable))]
	public interface ITrackable
	{
		//int ItemId { get; set; }
	//	string TrackingGuid { get; set; }
		string TrackingName { get; set; }
	}
}
