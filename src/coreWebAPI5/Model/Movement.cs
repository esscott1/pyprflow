using System.Collections.Generic;
using System.Runtime.Serialization;

namespace coreWebAPI5.Model
{
	public class Movement
	{
		public string From { get; set; }
		public string To { get; set; }
		public List<IUser> ApproveUsers { get; private set; }

		public Movement()
		{
			ApproveUsers = new List<IUser>();
		}
	}
}
