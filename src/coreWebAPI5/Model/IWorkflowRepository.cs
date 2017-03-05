using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace workflow.Model
{
	public interface IWorkflowRepository
	{

		IEnumerable<T> GetAll<T>();
		void Add(Workflow workflow);
		T Find<T>(string workflowItemId);
		void Remove<T>(string workflowItemId) where T : WorkflowItem;
		void Update<T>(T item) where T : WorkflowItem;


		void Add(Trackable trackable);
		
		
		
		void Add(Transaction trans);
		


		bool CheckValidUserKey(string stringValues);
	}
}
