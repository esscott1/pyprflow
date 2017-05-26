using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workflow.Model
{
    public class Orchestration : BaseWorkflowItem
    {
		public string OrchestrationName { get; set; }
		public List<Movement> Moves { get; set; }

		public bool IsValid(Transaction transaction)
		{
			Console.WriteLine("checking for validation in orchestration {0}", this.OrchestrationName);
			try
			{
				bool validpath = false; bool validrule = false;
				foreach(Movement m in Moves)
				{
					validpath = IsValidPath(m, transaction);
					validrule = IsValidRule(m, transaction);
					if (validpath && validrule)
					{
						Console.WriteLine("transaction is valid in orchestration {0}", this.OrchestrationName);
						return true;
					}
				}
				
			}
			catch(Exception ex)
			{
				Console.WriteLine("error in transaction validation in the orchestration class {0}", ex.Message);
				Console.WriteLine("inner exception {0}", ex.InnerException);
			}
			 return false; 
			 
		}
		private bool IsValidPath(Movement m, Transaction transaction)
		{
			return (m.From == transaction.CurrentNodeId && m.To == transaction.NewNodeId);
			

		}
		private bool IsValidRule(Movement m, Transaction transaction)
		{
			if (m.Rule == null)
				return true;
			return (m.Rule.IsValid(transaction));
		
		}
	}
}
