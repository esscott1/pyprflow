using System;

namespace MkDWorkFlow
{
	public interface ITrackable
	{
		int ItemId { get; }
		Guid TrackingGuid { get; set; }
		string TrackingName { get; set; }
	}
}
