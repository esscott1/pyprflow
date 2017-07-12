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

    public class SearchEngine
	{
       
		protected IWorkflowRepository Repository { get; set; }
		public SearchEngine(IWorkflowRepository repository)
		{
			Repository = repository;

		}

        public virtual List<BaseWorkflowItem> Search(SearchRequest request)
        {
            //if(request.EntityType == "workflows")
            SearchEngineContext context = new SearchEngineContext(Repository);
            return context.Search(request);
          
        }

    }

	
}
