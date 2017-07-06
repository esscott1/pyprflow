using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using pyprflow.Workflow.Model;

namespace pyprflow.Workflow.Db
{
    public class SearchRequestParameters
    {
        public Dictionary<string, string> Parameters = new Dictionary<string, string>()
        {
             { "entityType", string.Empty },
             { "trackableid", string.Empty },
             { "transactionid", string.Empty },
             { "workflowid", string.Empty },
             { "nodeid", string.Empty },
             { "assignedto", string.Empty },
             { "submittedby", string.Empty },
             { "transactiontype", string.Empty },
             { "isactive", string.Empty },
             { "etest", string.Empty }
        };

    }

    public class SearchRequest
    {
        public static Dictionary<string, Expression<Func<Database.Entity.Relationship, bool>>> _ClauseStrategy =
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
          //   { "transactiontype",  i => i.Type == Enum.Parse(typeof(Database.Entity.TransactionType)), _searchRequestParameters["transactiontype"]},
             { "isactive",  i => i.Active == bool.Parse(_searchRequestParameters["isactive"]) },
           };
        private static Dictionary<string, string> _searchRequestParameters;
        private Dictionary<string, Microsoft.Extensions.Primitives.StringValues> _requestParameters;
        public static readonly string[] SearchParameters = { "entitytype","trackableid","transactionid","workflowid",
            "nodeid",
            "assignedto","submittedby", "etest",
            "transactiontype",
            "isactive" };
        public static readonly string[] sEntityTypes = { "workflows", "trackables", "transactions", "trackablesenh" };

        public string EntityType { get; set; }
        public System.Linq.Expressions.Expression<Func<pyprflow.Database.Entity.Relationship, bool>> Predicate { get; set; }

        public Dictionary<string, Microsoft.Extensions.Primitives.StringValues> RequestParameters { get { return _requestParameters; } }
    
        public SearchRequest(Dictionary<string,string> searchRequestParameters)
        {
            string select; string sIsActive;
            searchRequestParameters.TryGetValue("entityType", out select);
            EntityType = select;
            searchRequestParameters.Remove("entityType");
            if (searchRequestParameters.TryGetValue("isactive", out sIsActive))
            {
                if (sIsActive == string.Empty)
                    searchRequestParameters["isactive"] = "true";// searchRequestParameters.Add("isactive", "true");
            }
            else
                searchRequestParameters.Add("isactive", "true");

            _searchRequestParameters = searchRequestParameters;
            eBuildPredictate2();
        }

        private void eBuildPredictate2()
        {
            var predicate = PredicateBuilder.True<pyprflow.Database.Entity.Relationship>();
            foreach (var kvp in _searchRequestParameters)
            {
                //if (kvp.Key == "entityType") // skip entityType as it's not a clauseable
                //    continue;
                if (kvp.Value != string.Empty)
                {
                    predicate = predicate.And(_ClauseStrategy[kvp.Key]);
                }
            }
            Predicate = predicate;
          

        }
        //      public SearchRequest(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryString)
        //{
        //          _requestParameters = queryString;
        //	StringValues select; StringValues sIsActive;
        //	queryString.TryGetValue("entityType", out select);
        //	EntityType = select;
        //	if (queryString.Count > 1)
        //	{
        //		if (queryString.TryGetValue("isactive", out sIsActive))
        //		{
        //			switch (sIsActive.FirstOrDefault().ToLower())
        //			{
        //				case "all":
        //					queryString.Remove("isactive");
        //					break;
        //				case "false":
        //					break;
        //				default:
        //					queryString.Remove("isactive");
        //					queryString.Add("isactive", "true");
        //					break;
        //			}

        //		}
        //		else
        //		{
        //			queryString.Add("isactive", "true");
        //		}

        //		BuildPredicate(queryString);
        //	}

        //}
        //      public void Init()
        //      {
        //          //_ClauseStrategy = new Dictionary<string, Expression<Func<Database.Entity.Relationship, bool>>>();
        //          //_ClauseStrategy.Add("nodeid", i => i.NodeName == _requestParameters["nodename"].First().ToString());
        //          //_ClauseStrategy.Add("transactionid", i => i.TransactionName ==);
        //      }

        //     private void eBuildPredicate(Dictionary<string,
        //         Microsoft.Extensions.Primitives.StringValues> queryString)
        //     {
        //         var innerpredicate = PredicateBuilder.True<pyprflow.Database.Entity.Relationship>();
        //         StringValues sIsActive = string.Empty;
        //         if (queryString.TryGetValue("isactive", out sIsActive))
        //         {
        //             //	Console.WriteLine("in if for try and get isactive value is {0}", sIsActive);
        //             bool bIsActive;
        //             Boolean.TryParse(sIsActive, out bIsActive);
        //             innerpredicate = innerpredicate.And(i => i.Active == bIsActive);
        //         }
        //         Init();
        //         innerpredicate = innerpredicate.And(_ClauseStrategy["etest1"]);
        //         innerpredicate = innerpredicate.And(_ClauseStrategy["etest2"]);

        //         //innerpredicate = innerpredicate.And(p => p.AssignedTo == "Joe.Worker@somewhere.com");
        //         //innerpredicate = innerpredicate.And(p => p.Type == Database.Entity.TransactionType.assignment);


        //         Predicate = innerpredicate;
        //     }

  //      private void BuildPredicate(Dictionary<string,
  //          Microsoft.Extensions.Primitives.StringValues> queryString)
  //      {
  //          if(queryString.ContainsKey("etest"))
  //          {
  //              eBuildPredicate(queryString);
  //              return;
  //          }
		//	var predicate = PredicateBuilder.True<pyprflow.Database.Entity.Relationship>();
		//	StringValues sIsActive = string.Empty;
		//	if (queryString.TryGetValue("isactive", out sIsActive))
		//	{
		//	//	Console.WriteLine("in if for try and get isactive value is {0}", sIsActive);
		//		bool bIsActive;
		//		Boolean.TryParse(sIsActive, out bIsActive);
		//		predicate = predicate.And(i => i.Active == bIsActive);
		//	}
			
		//	StringValues nodename;
		//	if (queryString.TryGetValue("nodeid", out nodename))
		//	{
		//		predicate = predicate.And(i => i.NodeName == nodename.ToString());

		//	}
		//	StringValues transactionName;
		//	if (queryString.TryGetValue("transactionid", out transactionName))
		//	{
		//		predicate = predicate.And(i => i.TransactionName == transactionName.ToString());
		//	}
		//	StringValues trackableName;
		//	if (queryString.TryGetValue("trackableid", out trackableName))
		//	{
		//		predicate = predicate.And(i => i.TrackableName == trackableName.ToString());
		//	}
		//	StringValues workflowName;
		//	if (queryString.TryGetValue("workflowid", out workflowName))
		//	{
		//		predicate = predicate.And(i => i.WorkflowName == workflowName.ToString());
		//	}
		//	StringValues assignedTo;
		//	if (queryString.TryGetValue("assignedto", out assignedTo))
		//	{
		//		predicate = predicate.And(i => i.AssignedTo == assignedTo.ToString());
		//	}
  //          StringValues submittedBy;
  //          if (queryString.TryGetValue("submittedby", out submittedBy))
  //          {
  //              predicate = predicate.And(i => i.Submitter == submittedBy.ToString());
  //          }
  //          StringValues sType;
		//	if (queryString.TryGetValue("transactiontype", out sType))
		//	{
		//		Console.WriteLine("found type of value {0}", sType);
		//		pyprflow.Database.Entity.TransactionType tType;
		//		if (Enum.TryParse<pyprflow.Database.Entity.TransactionType>(sType, out tType))
		//		{
		//			predicate = predicate.And(i => i.Type == tType);
		//			Console.WriteLine("tType value is {0}", tType.ToString());
		//		}
		//	}

		//	Predicate = predicate;

		//}
	}
}
