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
			switch (request.EntityType.ToLower())
			{
				case "workflows":
					return Repository.GetAll<Model.Workflow>().ToList().Cast<BaseWorkflowItem>().ToList();
					
				case "trackables":
					return Repository.GetAll<Trackable>().ToList().Cast<BaseWorkflowItem>().ToList();
					
				case "transactions":
					return Repository.GetAll<Transaction>().ToList().Cast<BaseWorkflowItem>().ToList();
					
				default:
					Console.WriteLine("{0} is not a valid SELECT keyword", request.EntityType);
					return null;
			}
		}

		private List<BaseWorkflowItem> SelectWithWhere( SearchRequest request)
		{
			Console.WriteLine("in select with where, select is {0}",request.EntityType);
			List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();
			try
			{
				List<Relationship> relationships = Repository.Where(request.Predicate);

				//TransactionType tType = TransactionType.assignment;
				//Console.WriteLine("overriden the search engine to search for {0}", tType.ToString());
				//relationships = Repository.Where(r => r.Type == tType);
				//
				//relationships.ForEach(r => { result.Add(Repository.Find<Transaction>(r.Type == tType)); });
				Console.WriteLine("going to search {0} of relationships",relationships.Count);

                switch (request.EntityType.ToLower())
                {
                    case "workflows":
                        var uniqueRWf = relationships.Select(x => x.WorkflowName).Distinct().ToList();
                        foreach (string wn in uniqueRWf)
                            result.Add(Repository.Find<Model.Workflow>(wn));
                        //relationships.ForEach(r => { result.Add(Repository.Find<Workflow>(r.WorkflowName)); });
                        break;
                    case "trackablesenh":
                        var uniqueR2 = relationships.Select(x => x.TrackableName).Distinct().ToList();
                        foreach (string tn in uniqueR2)
                        {
                            var trackable = Repository.Find<Trackable>(tn);
                            //var trackableSearchResult = Augment(trackable);
                            var trackableSearchResult = Augment(trackable,relationships);
                            result.Add(trackableSearchResult);
                        }
                            break;
                    case "trackables":
                        var uniqueR = relationships.Select(x => x.TrackableName).Distinct().ToList();
                        foreach (string tn in uniqueR)
                        {
                            //var trackable = Repository.Find<Trackable>(tn);
                            //var trackableSearchResult = new Model.SearchResult.TrackableSearchResult(trackable);
                            //result.Add(trackableSearchResult);
                            result.Add(Repository.Find<Trackable>(tn));
                        }
						//relationships.ForEach(r => { result.Add(Repository.Find<Trackable>(r.TrackableName)); });
						break;
					case "transactions":
						relationships.ForEach(r => { result.Add(Repository.Find<Transaction>(r.TransactionName)); });
						break;

					default:
						Console.WriteLine("{0} is not a valid SELECT keyword", request.EntityType);
						return null;

				}
			}
			catch(Exception ex)
			{
				Console.WriteLine("error in the switch {0}", ex.Message);
			}
			Console.WriteLine("results have {0}", result.Count);
			return result;
		}
		
		public List<BaseWorkflowItem> Search(SearchRequest request)
		{
			if (request.Predicate == null)
			{
				//Console.WriteLine("predicate is null");
				return SelectWithoutWhere(request);
			}
			//Console.WriteLine("predicate is not null, looking for {0}", request.EntityType.ToString());
			 var result = SelectWithWhere(request);
			//Console.WriteLine("found this many relationships {0}", result.Count);
			return result;
		}

        private TrackableSearchResult Augment(Trackable trackable, List<Relationship> relationships)
        {
            
            TrackableSearchResult result = new TrackableSearchResult(trackable);
                foreach (Relationship rel in relationships.Where(r => r.TrackableName==result.Name))
                {

                    if (rel.Type == TransactionType.copy || rel.Type == TransactionType.move)
                        result.Locations.Add(rel.NodeName);
                    if (rel.Type == TransactionType.assignment)
                        result.CurrentAssignment.Add(rel.AssignedTo);
                    if (rel.Type == TransactionType.comment)
                        result.Comments.Add(rel.Comment);
                }
            return result;
        }

      
         
                

       
	}

	
}
