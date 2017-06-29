using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyprflow.Workflow.Model
{
    public class Rule : BaseWorkflowItem
    {
		// those that can execute this rule
		public List<User> AccessList { get; set; }

		internal bool IsValid(Transaction transaction)
		{
			try
			{
				Console.WriteLine("the orch has these auth users {0}", this.AccessList.First().Email);
				Console.WriteLine("the transaction  has these auth users {0}", transaction.Submitter.Email);
				foreach(User u in this.AccessList )
				{
					if (u.Email == transaction.Submitter.Email)
						return true;
				}
				return false;
				//return this.AccessList.Exists(u => u.Email == transaction.Submitter.Email);
			}
			catch(Exception ex)
			{
				Console.WriteLine("error checking IsValid in rule {0}", ex.Message);
				Console.WriteLine("inner exception {0}", ex.InnerException);
				return false;
			}
			
		}
	}
}
