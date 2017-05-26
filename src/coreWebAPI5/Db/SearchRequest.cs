using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using workflow.Model;

namespace workflow.Db
{
    public class SearchRequest
	{
		public string EntityType { get; set; }
		public System.Linq.Expressions.Expression<Func<Relationship, bool>> Predicate { get; set; }

		public SearchRequest(Dictionary<string,
			Microsoft.Extensions.Primitives.StringValues> queryString)
		{
			StringValues select; StringValues sIsActive;
			queryString.TryGetValue("entityType", out select);
			EntityType = select;
			if (queryString.Count > 1)
			{
				if (queryString.TryGetValue("isactive", out sIsActive))
				{
					switch (sIsActive.FirstOrDefault().ToLower())
					{
						case "all":
							queryString.Remove("isactive");
							break;
						case "false":
							break;
						default:
							queryString.Remove("isactive");
							queryString.Add("isactive", "true");
							break;
					}
					
				}
				else
				{
					queryString.Add("isactive", "true");
				}
			
				BuildPredicate(queryString);
			}

		}
		private void BuildPredicate(Dictionary<string,
			Microsoft.Extensions.Primitives.StringValues> queryString)
		{
			var predicate = PredicateBuilder.True<Relationship>();
			StringValues sIsActive = string.Empty;
			if (queryString.TryGetValue("isactive", out sIsActive))
			{
			//	Console.WriteLine("in if for try and get isactive value is {0}", sIsActive);
				bool bIsActive;
				Boolean.TryParse(sIsActive, out bIsActive);
				predicate = predicate.And(i => i.Active == bIsActive);
			}
			
			StringValues nodename;
			if (queryString.TryGetValue("nodeid", out nodename))
			{
				predicate = predicate.And(i => i.NodeName == nodename.ToString());

			}
			StringValues transactionName;
			if (queryString.TryGetValue("transactionid", out transactionName))
			{
				predicate = predicate.And(i => i.TransactionName == transactionName.ToString());
			}
			StringValues trackableName;
			if (queryString.TryGetValue("trackableid", out trackableName))
			{
				predicate = predicate.And(i => i.TrackableName == trackableName.ToString());
			}
			StringValues workflowName;
			if (queryString.TryGetValue("workflowid", out workflowName))
			{
				predicate = predicate.And(i => i.WorkflowName == workflowName.ToString());
			}
			StringValues assignedTo;
			if (queryString.TryGetValue("assignedto", out assignedTo))
			{
				predicate = predicate.And(i => i.AssignedTo == assignedTo.ToString());
			}
			StringValues sType;
			if (queryString.TryGetValue("transactiontype", out sType))
			{
				Console.WriteLine("found type of value {0}", sType);
				TransactionType tType;
				if (Enum.TryParse<TransactionType>(sType, out tType))
				{
					predicate = predicate.And(i => i.Type == tType);
					Console.WriteLine("tType value is {0}", tType.ToString());
				}
			}

			Predicate = predicate;

		}
	}
}
