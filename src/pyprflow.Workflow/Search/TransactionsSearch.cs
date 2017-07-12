using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using pyprflow.Workflow.Model;

namespace pyprflow.Workflow.Search
{
    class TransactionsSearch : SearchEngine, ISearchEngine
    {
        public TransactionsSearch(IWorkflowRepository repository) : base(repository)
        {
        }

        public override List<BaseWorkflowItem> Search(SearchRequest request)
        {
            List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();
            if (request.Predicate == null)
            {
                result = Repository.GetAll<Transaction>().ToList().Cast<BaseWorkflowItem>().ToList();
            }
           var  tmpresult = Repository.Where<Model.Transaction>(request.Predicate);
            if (tmpresult == null)
                return result;
            result = tmpresult.Where(s => s !=null).Cast<BaseWorkflowItem>().ToList();


            return result;
        }
    }
}
