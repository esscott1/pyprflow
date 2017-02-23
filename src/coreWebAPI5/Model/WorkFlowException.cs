using System;

namespace workflow.Model
{
	public class WorkFlowException : Exception
	{
		public WorkFlowException() { }

		public WorkFlowException(string message) : base(message) { }

		public WorkFlowException(string message, Exception inner) : base(message, inner) { }
	}
}
