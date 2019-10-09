using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyprflow.Workflow.Model
{
    public class Transaction : BaseWorkflowItem 
	{
		
		public string TrackableName { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public TransactionType type { get; set; }
		
		public string CurrentNodeId { get; set; }
		
		public string NewNodeId { get; set; }
        public string WorkflowName { get; set; }
        public DateTime TransActionTime { get; internal set; }
		[JsonRequired]
		public User Submitter { get; set; }
		public User AssignedTo { get; set; }
        public string Comment { get; set; }
       

      //  private IWorkflowRepository Repository;
		public Transaction()
		{
			TransActionTime = DateTime.Now;
            if (TransActionTime.Year.Equals(1)) // the null DateTime
                TransActionTime = DateTime.Now;
           
		}
  
		private void Clean()
		{
			if(this.type== TransactionType.move || this.type==TransactionType.copy)
			{
				this.AssignedTo = null;
				//this.Comment = null;
			}
			if(this.type == TransactionType.comment) {
				this.NewNodeId = null;
				this.AssignedTo = null;
			}

			if (this.type == TransactionType.assignment)
			{
				this.NewNodeId = null;
				//this.Comment = null;
			}
		}

		internal bool Execute(IWorkflowRepository repository, out int statuscode, out string statusmessage)
		{
			this.Clean();
            TransactionValidator validator = new TransactionValidator(repository);
            bool result = true;
            result = validator.IsValid(this, out statuscode, out statusmessage);
            if (!result)
                return result;
   
			Console.WriteLine("executing the transaction");
			statuscode = 0;
			statusmessage = "success";
			var workflow = repository.Find<Workflow>(WorkflowName);

			// should the transaction object execute
			//trans.Execute();
			if (this.type == TransactionType.copy || this.type == TransactionType.move)
			{
				if (!workflow.IsMoveValid(this, repository))
				{
					statuscode = 400;
					statusmessage = "move is not valid per workflow rules";
					return false;
				}
				Console.WriteLine("passed IsValid validation");
				
			}
			repository.Add(this);
            
			return true;
		}
	
	
    
		
	}

	
}
