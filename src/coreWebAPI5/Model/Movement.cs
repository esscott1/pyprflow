using System.Collections.Generic;
using System.Runtime.Serialization;

namespace workflow.Model
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
