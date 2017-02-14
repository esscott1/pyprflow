using System;

namespace MkDWorkFlow
{
	public class WorkFlowException : Exception
	{
		public WorkFlowException() { }

		public WorkFlowException(string message) : base(message) { }

		public WorkFlowException(string message, Exception inner) : base(message, inner) { }
	}
}
