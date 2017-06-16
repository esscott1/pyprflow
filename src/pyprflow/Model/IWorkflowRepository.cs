using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace pyprflow.Model
{
	public interface IWorkflowRepository
	{

		IEnumerable<T> GetAll<T>();
		void Add(Workflow workflow);
		T Find<T>(string  name);
		void Remove<T>(string workflowItemId) where T : BaseWorkflowItem;
		void Update<T>(T item) where T : BaseWorkflowItem;


		void Add(Trackable trackable);

		List<Relationship> Where(System.Linq.Expressions.Expression<Func<Relationship, bool>> predicate);


		void Add(Transaction trans);
		


		bool CheckValidUserKey(string stringValues);
		void Track(Transaction trans);
        
        void EmptyAll();
    }
}
