using System.Collections.Generic;
using System.Runtime.Serialization;

namespace workflow.Model
{
	public class Movement
	{
		public string From { get; set; }
		public string To { get; set; }
		public List<User> ApproveUsers { get; internal set; }
		//path that also need to occur based on business rule definition
		//public List<Movement> TriggeredMoves { get; set; }
		public Movement()
		{
			ApproveUsers = new List<User>();
		}
		public Movement(Movement init)
		{
			this.From = init.From;
			this.To = init.To;
			this.ApproveUsers = init.ApproveUsers;
		}
	}
}
