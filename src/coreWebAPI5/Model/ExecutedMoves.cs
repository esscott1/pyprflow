using System;


namespace workflow.Model
{
	public class ExecutedMove : Model.Movement
	{
		public string Key { get; set; }
		public string TrackableId { get; set; }
		public ExecutedMove(Movement movement) : base(movement) { }
		public DateTime ExecutionTime { get; set; }
		public IUser Mover { get; set; }
		public string Comment { get; set; }
		public ExecutedMove(string key)
		{
			Key = key;
		}
	}
	
}