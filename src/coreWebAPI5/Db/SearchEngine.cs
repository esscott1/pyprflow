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
		//public Dictionary<string,string> Where { get; set; }
		public string Select { get; set; }
		public bool Active { get; set; } = true;
		public System.Linq.Expressions.Expression<Func<Relationship, bool>> Predicate { get; set; }

		public SearchRequest(Dictionary<string,
			Microsoft.Extensions.Primitives.StringValues> queryString)
		{
			//Where = new Dictionary<string, string>();
			
			StringValues select;
			queryString.TryGetValue("entityType", out select);
			Select = select;
			if(queryString.Count>1)
				BuildPredicate(queryString);
			
		}
		private void BuildPredicate(Dictionary<string,
			Microsoft.Extensions.Primitives.StringValues> queryString)
		{
			var predicate = PredicateBuilder.True<Relationship>();
			StringValues sIsActive = string.Empty;
			if(queryString.TryGetValue("isactive",out sIsActive)) 
			{
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

			Predicate = predicate;

		}
	}

	public class SearchEngine
	{
		private IWorkflowRepository Repository { get; set; }
		public SearchEngine(IWorkflowRepository repository)
		{
			Repository = repository;
		}

		private List<Relationship> GetRelationships(string type, string whereValue, bool active)
		{
			Console.WriteLine("GetRelationships active flag is {0}", active);
			List<Relationship> relationships = new List<Relationship>();
			switch (type)
			{
				case "workflowid":
					relationships = Repository.Where(r => r.WorkflowName == whereValue && r.Active == active);
					break;
				case "trackableid":
					relationships = Repository.Where(r => r.TrackableName == whereValue && r.Active == active);
					break;
				case "transactionid":
					relationships = Repository.Where(r => r.TransactionName == whereValue && r.Active == active);
					break;
				case "nodeid":
					relationships = Repository.Where(r => r.NodeName == whereValue && r.Active == active);
					break;
				default:
					Console.WriteLine("{0} is not a valid WHERE keyword", whereValue);
					return null;
			}
			Console.WriteLine("found {0} relationships ", relationships.Count);
			return relationships;

		}

		private List<BaseWorkflowItem> SelectWithoutWhere(SearchRequest request)
		{
			//List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();
			switch (request.Select.ToLower())
			{
				case "workflows":
					return Repository.GetAll<Workflow>().ToList().Cast<BaseWorkflowItem>().ToList();
					
				case "trackables":
					return Repository.GetAll<Trackable>().ToList().Cast<BaseWorkflowItem>().ToList();
					
				case "transactions":
					return Repository.GetAll<Transaction>().ToList().Cast<BaseWorkflowItem>().ToList();
					
				default:
					Console.WriteLine("{0} is not a valid SELECT keyword", request.Select);
					return null;
			}

			throw new NotImplementedException();
		}

		private List<BaseWorkflowItem> SelectWithWhere( SearchRequest request)
		{
			
			List<Relationship> relationships = Repository.Where(request.Predicate);
			List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();
			switch (request.Select.ToLower())
			{
				case "workflows":
					relationships.ForEach(r => { result.Add(Repository.Find<Workflow>(r.WorkflowName)); });
					break;
				case "trackables":
					relationships.ForEach(r => { result.Add(Repository.Find<Trackable>(r.TrackableName)); });
					break;
				case "transactions":
					relationships.ForEach(r => { result.Add(Repository.Find<Transaction>(r.TransactionName)); });
					break;
				default:
					Console.WriteLine("{0} is not a valid SELECT keyword", request.Select);
					return null;

			}
			return result;
		}
		
		public List<BaseWorkflowItem> Search(SearchRequest request)
		{
			if (request.Predicate == null)
			{
				return SelectWithoutWhere(request);
			}
			return SelectWithWhere(request);
			//List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();
			//List<string> types = new List<string> { "workflowid", "trackableid", "transactionid", "nodeid" };
			//string whereValue = string.Empty; string type = string.Empty;
			//foreach (string t in types)
			//{
			//	if (request.Where.TryGetValue(t, out whereValue))
			//	{
			//		type = t;
			//		break;
			//	}
			//}
			//List<Relationship> relationships = GetRelationships(type, whereValue, request.Active);

			// issue that without where clause i have nothing to search for.
			//return SelectWithWhere(relationships, request);
			//return result;

		}


	}

	
}
