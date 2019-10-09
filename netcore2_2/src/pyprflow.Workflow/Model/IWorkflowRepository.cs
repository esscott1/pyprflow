using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace pyprflow.Workflow.Model
{
	public interface IWorkflowRepository
	{

        IEnumerable<T> GetAll<T>() where T : BaseWorkflowItem;

        void Add<T>(T item) where T : BaseWorkflowItem;
        T Find<T>(string name) where T : BaseWorkflowItem;
        void HardDelete<T>(string workflowItemId) where T : BaseWorkflowItem;
		void Update<T>(T item) where T : BaseWorkflowItem;
        void SoftDelete<T>(string workflowItemId) where T : BaseWorkflowItem;
        bool Exist<T>(string itemName) where T : BaseWorkflowItem;

        //List<Relationship> Where(System.Linq.Expressions.Expression<Func<Relationship, bool>> predicate);
        List<Relationship> Where(System.Linq.Expressions.Expression<Func<pyprflow.DbEntity.Relationship, bool>> predicate);
        List<T> Where<T>(System.Linq.Expressions.Expression<Func<pyprflow.DbEntity.Relationship, bool>> predicate) where T : BaseWorkflowItem;
        List<BaseWorkflowItem> Where<T>(System.Linq.Expressions.Expression<Func<pyprflow.DbEntity.Relationship, bool>> predicate, Type returnType) where T : BaseWorkflowItem;
        bool CheckValidUserKey(string stringValues);
        bool Execute(Transaction trans, out int statusCode, out string msg);
        
        void EmptyAll();
    }
}
