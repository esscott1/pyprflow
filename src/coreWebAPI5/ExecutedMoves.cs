using System;


namespace workflow.Model
{
	public class ExecutedMove : Model.Movement
	{
		public ExecutedMove(Movement movement) : base(movement) { }
		public DateTime ExecutionTime { get; set; }
		public IUser Mover { get; set; }
		public string Comment { get; set; }
	}
}