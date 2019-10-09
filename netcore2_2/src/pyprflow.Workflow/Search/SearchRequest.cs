using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using pyprflow.Workflow.Model;
using System.Runtime.InteropServices;

namespace pyprflow.Workflow.Search
{
    public enum EntityType
    {
        workflows, 
        trackables, 
        transactions, 
        trackablesenh
    }
    public enum IsActive
    {
        
        @true = 0,
        @false = 1,
        both = 2
    }
    public class SearchRequestParameters
    {
        
        public Dictionary<string, string> Parameters = new Dictionary<string, string>()
        {
           //  { "entityType", string.Empty },
             { "trackableid", string.Empty },
             { "transactionid", string.Empty },
             { "workflowid", string.Empty },
             { "nodeid", string.Empty },
             { "assignedto", string.Empty },
             { "submittedby", string.Empty },
           //  { "transactiontype", string.Empty },
           //  { "isactive", string.Empty },
             { "etest", string.Empty }
        };

        public pyprflow.Workflow.Model.TransactionType transactiontype { get; set; }
        public IsActive isActive { get; set; }
    }
   

    public class SearchRequest
    {
        public Type GetReturnType() {
            Type result;
            switch(_entityType){
                case Search.EntityType.trackables:
                    result = Type.GetType("pyprflow.Workflow.Model.Trackable, pyprflow.Workflow");
                    break;
                case Search.EntityType.trackablesenh:
                    result = Type.GetType("pyprflow.Workflow.Model.Trackable, pyprflow.Workflow");
                    break;
                case Search.EntityType.transactions:
                    result = Type.GetType("pyprflow.Workflow.Model.Transaction, pyprflow.Workflow");
                    break;
                case Search.EntityType.workflows:
                    result = Type.GetType("pyprflow.Workflow.Model.Workflow, pyprflow.Workflow");
                    break;
                default:
                    result = Type.GetType("pyprflow.Workflow.Model.BaseWorkflowItem, pyprflow.Workflow");
                    break;

            }
            return result;
            //return Type.GetType("pyprflow.Workflow.Model.Trackable, pyprflow.Workflow");
        }

        internal EntityType _entityType { get; private set; }
        public static pyprflow.Workflow.Model.TransactionType type;// = default(Database.Entity.TransactionType);
        internal static Dictionary<string, Expression<Func<DbEntity.Relationship, bool>>> _ClauseStrategy =
           new Dictionary<string, Expression<Func<DbEntity.Relationship, bool>>>()
           {
              //  { "etest",  i => i.NodeName == _searchRequestParameters["nodeid"]},
              //{ "entitytype", i => i.NodeName == _searchRequestParameters["nodeid"] },
             { "trackableid",  i => i.TrackableName == _searchRequestParameters["trackableid"] },
             { "transactionid",  i => i.TransactionName == _searchRequestParameters["transactionid"] },
             { "workflowid",  i => i.WorkflowName == _searchRequestParameters["workflowid"] },
             { "nodeid",  i => i.NodeName == _searchRequestParameters["nodeid"]},
             { "assignedto", i => i.AssignedTo == _searchRequestParameters["assignedto"]},
             { "submittedby",  i => i.Submitter == _searchRequestParameters["submittedby"]},
             { "transactiontype",  i => i.Type == type}, 
             { "isactive",  i => i.Active == bool.Parse(_searchRequestParameters["isactive"]) },
           };
        private static Dictionary<string, string> _searchRequestParameters;
     

        internal string EntityType { get; set; }
        internal System.Linq.Expressions.Expression<Func<pyprflow.DbEntity.Relationship, bool>> Predicate { get; set; }

        public SearchRequest(SearchRequestParameters searchRequestParameters, EntityType entityType)
        {
            _entityType = entityType;
            EntityType = entityType.ToString();
            string sIsActive = searchRequestParameters.isActive.ToString();
            if(sIsActive == "true" || sIsActive=="false")
                searchRequestParameters.Parameters.Add("isactive" ,sIsActive);
            if (searchRequestParameters.transactiontype != pyprflow.Workflow.Model.TransactionType.none)
            {
                searchRequestParameters.Parameters.Add("transactiontype", searchRequestParameters.transactiontype.ToString());
                type = searchRequestParameters.transactiontype;
            }
              

            _searchRequestParameters = searchRequestParameters.Parameters;
            eBuildPredictate2();
        }

        private void eBuildPredictate2()
        {
            var predicate = PredicateBuilder.True<pyprflow.DbEntity.Relationship>();
            foreach (var kvp in _searchRequestParameters)
            {
                if (kvp.Value != string.Empty)
                {
                    predicate = predicate.And(_ClauseStrategy[kvp.Key]);
                }
            }
            Predicate = predicate;
          

        }
       
	}
}
