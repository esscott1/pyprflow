using System.Collections.Generic;
using pyprflow.Workflow.Model;

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

	
}
