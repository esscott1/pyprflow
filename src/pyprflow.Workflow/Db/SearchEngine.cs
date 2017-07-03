using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using pyprflow.Workflow.Model;
using pyprflow.Workflow.Model.SearchResult;
using pyprflow.Workflow;

namespace pyprflow.Workflow.Db
{
    public class SearchEngineContext
    {
        private readonly Dictionary<string, ISearchEngine> _strategies =
           new Dictionary<string, ISearchEngine>();

        protected IWorkflowRepository Repository { get; set; }
        public SearchEngineContext(IWorkflowRepository repository)
        {
            _strategies = new Dictionary<string, ISearchEngine>();
            _strategies.Add("workflows", new WorkflowsSearch(repository));
            _strategies.Add("trackables", new TrackablesSearch(repository));
            _strategies.Add("transactions", new TransactionsSearch(repository));
            _strategies.Add("trackablesenh", new TrackablesEnhSearch(repository));
        }
        public List<BaseWorkflowItem> Search(SearchRequest request)
        {
            return _strategies[request.EntityType].Search(request);
        }

    }

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
