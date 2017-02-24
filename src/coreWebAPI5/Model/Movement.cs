using System.Collections.Generic;
using System.Runtime.Serialization;

namespace workflow.Model
{
	public class Movement
	{
		public string From { get; set; }
		public string To { get; set; }
		public List<IUser> ApproveUsers { get; private set; }
		//path that also need to occur based on business rule definition
		//public List<Movement> TriggeredMoves { get; set; }
		public Movement()
		{
			ApproveUsers = new List<IUser>();
		}
		public Movement(Movement init)
		{
			this.From = init.From;
			this.To = init.To;
			this.ApproveUsers = init.ApproveUsers;
		}
	}
}
