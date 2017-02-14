using System;

namespace coreWebAPI5.Model
{
	public interface ITrackable
	{
		int ItemId { get; }
		Guid TrackingGuid { get; set; }
		string TrackingName { get; set; }
	}
}
