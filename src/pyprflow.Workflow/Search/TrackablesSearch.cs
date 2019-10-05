using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using pyprflow.Workflow.Model;

namespace pyprflow.Workflow.Search
{
    class TrackablesSearch : SearchEngine, ISearchEngine
    {
        public TrackablesSearch(IWorkflowRepository repository) : base(repository)
        {
        }

        //public override List<BaseWorkflowItem> Search(SearchRequest request)
        //{
        //    List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();
        //    if (request.Predicate == null)
        //    {
        //        Type t1 = request.GetReturnType();
        //        // For non-public methods, you'll need to specify binding flags too
              
        //      //  result = Repository.GetAll<Trackable>().ToList().Cast<BaseWorkflowItem>().ToList();
        //        result = Repository.GetAll<BaseWorkflowItem>().ToList().Cast<BaseWorkflowItem>().ToList();
        //        return result;
        //    }
        //    //var stuff = Repository.Where<Trackable>(request.Predicate);//.Where(s => s != null);
        //    var stuff = Repository.Where<BaseWorkflowItem>(request.Predicate,request.GetReturnType());//.Where(s => s != null);
        //    if (stuff !=null)
        //         result = stuff.Cast<BaseWorkflowItem>().ToList();
        //  //  result = Repository.Where<Trackable>(request.Predicate).Cast<BaseWorkflowItem>().ToList();
        //    return result;
        //}
    }
}
