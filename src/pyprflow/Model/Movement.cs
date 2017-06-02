using System.Collections.Generic;
using System.Runtime.Serialization;

namespace pyprflow.Model
{
	public class Movement
	{
		public string From { get; set; }
		public string To { get; set; }
		public Rule Rule { get; set; }
		//Orchestrations that also need to occur based on business rule definition
		//public List<Movement> TriggeredMoves { get; set; }
		public Movement()
		{

		}
		public Movement(Movement init)
		{
			this.From = init.From;
			this.To = init.To;
		}
	}
}
