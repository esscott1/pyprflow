using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coreWebAPI5.Model;

namespace coreWebAPI5.Model.Fakes
{
	public class TrackableDocument : ITrackable
	{
		public Guid TrackingGuid { get; set; }
		public int ItemId { get; set; }
		public string TrackingName { get; set; }
		public TrackableDocument()
		{
			if (TrackingGuid == Guid.Empty)
			{
				TrackingGuid = Guid.NewGuid();
			}
		}
		public TrackableDocument(string trackingName) : this()
		{
			TrackingName = trackingName;

		}
	}
}