using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using pyprflow.Workflow.Model;
using System.Runtime.InteropServices;

namespace pyprflow.Workflow.Db
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

        public Database.Entity.TransactionType transactiontype { get; set; }
        public IsActive isActive { get; set; }
    }
   

    public class SearchRequest
    {
        public static Database.Entity.TransactionType type;// = default(Database.Entity.TransactionType);
        internal static Dictionary<string, Expression<Func<Database.Entity.Relationship, bool>>> _ClauseStrategy =
           new Dictionary<string, Expression<Func<Database.Entity.Relationship, bool>>>()
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
        internal System.Linq.Expressions.Expression<Func<pyprflow.Database.Entity.Relationship, bool>> Predicate { get; set; }

        public SearchRequest(SearchRequestParameters searchRequestParameters, EntityType entityType)
        {
            EntityType = entityType.ToString();
            string sIsActive = searchRequestParameters.isActive.ToString();
            if(sIsActive == "true" || sIsActive=="false")
                searchRequestParameters.Parameters.Add("isactive" ,sIsActive);
            if (searchRequestParameters.transactiontype != Database.Entity.TransactionType.none)
            {
                searchRequestParameters.Parameters.Add("transactiontype", searchRequestParameters.transactiontype.ToString());
                type = searchRequestParameters.transactiontype;
            }
              

            _searchRequestParameters = searchRequestParameters.Parameters;
            eBuildPredictate2();
        }

        private void eBuildPredictate2()
        {
            var predicate = PredicateBuilder.True<pyprflow.Database.Entity.Relationship>();
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
