using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using pyprflow.Workflow.Model;

namespace pyprflow.Workflow.Search
{
    class WorkflowsSearch: SearchEngine, ISearchEngine
    {
        public WorkflowsSearch(IWorkflowRepository repository) : base(repository)
        {
        }

        //public override List<BaseWorkflowItem> Search(SearchRequest request)
        //{
        //    List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();
        //    if (request.Predicate == null) { 
        //       // result = Repository.GetAll<Model.Workflow>().ToList().Cast<BaseWorkflowItem>().ToList();
        //        result = Repository.GetAll<Model.BaseWorkflowItem>().ToList().Cast<BaseWorkflowItem>().ToList();
        //        return result;
        //     }
        //   // var tmpresult = Repository.Where<Model.Workflow>(request.Predicate);
        //    var tmpresult = Repository.Where<Model.BaseWorkflowItem>(request.Predicate, request.GetReturnType());
        //    if (tmpresult == null)
        //        return null;
        //    result = tmpresult.Cast<BaseWorkflowItem>().ToList();
        //    return result;
        //}
    }
}
