using System;
using System.Collections.Generic;
using System.Linq;
using pyprflow.Workflow.Model;


namespace pyprflow.Workflow.Search
{
    internal interface ISearchEngine
    {
        List<BaseWorkflowItem> Search(SearchRequest request);
    }
}