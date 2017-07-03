using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using pyprflow.Workflow.Model;

namespace pyprflow.Workflow.Db
{
    class TrackablesSearch : SearchEngine, ISearchEngine
    {
        public TrackablesSearch(IWorkflowRepository repository) : base(repository)
        {
        }

        public override List<BaseWorkflowItem> Search(SearchRequest request)
        {
            List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();
            if (request.Predicate == null)
            {
                result = Repository.GetAll<Trackable>().ToList().Cast<BaseWorkflowItem>().ToList();
                return result;
            }
            result = Repository.Where<Trackable>(request.Predicate).Cast<BaseWorkflowItem>().ToList();
            return result;
        }
    }
}
