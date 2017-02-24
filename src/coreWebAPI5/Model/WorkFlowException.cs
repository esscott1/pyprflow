using System;
using System.Collections.Generic;

namespace workflow.Model
{
	public class WorkFlowException : Exception
	{
		public WorkFlowException() { }

		public WorkFlowException(string message) : base(message) { }

		public WorkFlowException(string message, Exception inner) : base(message, inner) { }
	}
	
	public class WorkflowValidationMessage
	{
		public bool HasStart  { get; set; }
		public bool HasEnd { get; set; }
		public List<string> UnreachableNodeNames { get; set; }
		public List<string> DeadEndNodeNames { get; set; }

		public bool Valid { get { return isValid(); } }
		public WorkflowValidationMessage()
		{
			HasStart = false;
			HasEnd = false;
			UnreachableNodeNames = new List<string>();
			DeadEndNodeNames = new List<string>();
		}
		public bool isValid()
		{
			if (HasStart && HasEnd &&
				UnreachableNodeNames.Count == 0 &&
				DeadEndNodeNames.Count == 0)
				return true;
			return false;

		}
	}
}
