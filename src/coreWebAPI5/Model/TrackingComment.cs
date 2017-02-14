using Newtonsoft.Json;
using System;

namespace coreWebAPI5.Model
{
	public class TrackingComment
	{
		public DateTime Time { get; private set; }
		public string Text { get; private set; }
		public IUser User { get; private set; }
		public Guid DocId { get; private set; }

		[JsonConstructor]
		public TrackingComment(string comment, IUser user, Guid docId)
		{
			Time = DateTime.Now;
			Text = comment;
			User = user;
			DocId = docId;
		}
	}
}
