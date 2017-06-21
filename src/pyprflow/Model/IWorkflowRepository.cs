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

        IEnumerable<T> GetAll<T>() where T : BaseWorkflowItem;

        void Add<T>(T item) where T : BaseWorkflowItem;
        T Find<T>(string name) where T : BaseWorkflowItem;
        void Remove<T>(string workflowItemId) where T : BaseWorkflowItem;
		void Update<T>(T item) where T : BaseWorkflowItem;
        void Deactivate<T>(string workflowItemId) where T : BaseWorkflowItem;


        //List<Relationship> Where(System.Linq.Expressions.Expression<Func<Relationship, bool>> predicate);
        List<Relationship> Where(System.Linq.Expressions.Expression<Func<pyprflow.Database.Entity.Relationship, bool>> predicate);

        bool CheckValidUserKey(string stringValues);
      
        
        void EmptyAll();
    }
}
