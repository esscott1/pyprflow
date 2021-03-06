﻿using System.Collections.Generic;
using pyprflow.Workflow.Model;

namespace pyprflow.Workflow.Search
{
    public class SearchEngineContext
    {
        private readonly Dictionary<string, ISearchEngine> _strategies =
           new Dictionary<string, ISearchEngine>();

        protected IWorkflowRepository Repository { get; set; }
        public SearchEngineContext(IWorkflowRepository repository)
        {
            _strategies = new Dictionary<string, ISearchEngine>();
            _strategies.Add("workflows", new SearchEngine(repository));
            _strategies.Add("trackables", new SearchEngine(repository));
            _strategies.Add("transactions", new SearchEngine(repository));
           // _strategies.Add("trackablesenh", new TrackablesEnhSearch(repository));
        }
        public List<BaseWorkflowItem> Search(SearchRequest request)
        {
            return _strategies[request.EntityType].Search(request);
        }

    }

	
}
