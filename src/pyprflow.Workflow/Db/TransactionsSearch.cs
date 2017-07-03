using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using pyprflow.Workflow.Model;

namespace pyprflow.Workflow.Db
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
            result = Repository.Where<Model.Transaction>(request.Predicate).Cast<BaseWorkflowItem>().ToList();


            return result;
        }
    }
}
