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
		T Find<T>(int workflowItemId);
		void Remove<T>(int workflowItemId) where T : BaseWorkflowItem;
		void Update<T>(T item) where T : BaseWorkflowItem;


		void Add(Trackable trackable);
		
		
		
		void Add(Transaction trans);
		


		bool CheckValidUserKey(string stringValues);
	}
}
