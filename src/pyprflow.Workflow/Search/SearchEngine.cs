using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using pyprflow.Workflow.Model;
using pyprflow.Workflow.Model.SearchResult;
using pyprflow.Workflow;

namespace pyprflow.Workflow.Search
{

    public class SearchEngine: ISearchEngine
	{
       
		protected IWorkflowRepository Repository { get; set; }
		public SearchEngine(IWorkflowRepository repository)
		{
			Repository = repository;

		}

        public List<BaseWorkflowItem> Search(SearchRequest request)
        {
            List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();
            if (request.Predicate == null)
            {
                // result = Repository.GetAll<Model.Workflow>().ToList().Cast<BaseWorkflowItem>().ToList();
                result = Repository.GetAll<Model.BaseWorkflowItem>().ToList().Cast<BaseWorkflowItem>().ToList();
                return result;
            }
            // var tmpresult = Repository.Where<Model.Workflow>(request.Predicate);
            var tmpresult = Repository.Where<Model.BaseWorkflowItem>(request.Predicate, request.GetReturnType());
            if (tmpresult == null)
                return null;
            result = tmpresult.Cast<BaseWorkflowItem>().ToList();
            return result;
        }

    }

	
}
