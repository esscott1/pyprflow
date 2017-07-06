using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using pyprflow.Workflow.Model;

namespace pyprflow.Workflow.Db
{
    class WorkflowsSearch: SearchEngine, ISearchEngine
    {
        public WorkflowsSearch(IWorkflowRepository repository) : base(repository)
        {
        }

        public override List<BaseWorkflowItem> Search(SearchRequest request)
        {
            List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();
            if (request.Predicate == null) { 
                result = Repository.GetAll<Model.Workflow>().ToList().Cast<BaseWorkflowItem>().ToList();
                return result;
             }
            var tmpresult = Repository.Where<Model.Workflow>(request.Predicate);
            if (tmpresult == null)
                return null;
            result = tmpresult.Cast<BaseWorkflowItem>().ToList();
            return result;
        }
    }
}
