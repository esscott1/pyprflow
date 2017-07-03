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
        internal static Dictionary<string, ISearchEngine> _strategies =
            new Dictionary<string, ISearchEngine>();
		protected IWorkflowRepository Repository { get; set; }
		public SearchEngine(IWorkflowRepository repository)
		{
			Repository = repository;

		}

        public virtual List<BaseWorkflowItem> Search(SearchRequest request)
        {
            if(request.EntityType == "workflows")
               return  _strategies[request.EntityType].Search(request);

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

        public List<T> Search<T>(SearchRequest request)
        {
            return new List<T>();
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
            //todo refactor to minimize switches to the ones in the Workflow Repository.
          //  result = Repository.Where<T>(request.Predicate).Cast<BaseWorkflowItem>().ToList();
			try
			{
				List<Relationship> relationships = Repository.Where(request.Predicate);
				
				Console.WriteLine("going to search {0} of relationships",relationships.Count);

                switch (request.EntityType.ToLower())
                {
                    case "workflows":
                        var uniqueRWf = relationships.Select(x => x.WorkflowName).Distinct().ToList();
                        foreach (string wn in uniqueRWf)
                            result.Add(Repository.Find<Model.Workflow>(wn));
                        //relationships.ForEach(r => { result.Add(Repository.Find<Workflow>(r.WorkflowName)); });
                        break;
                  
                    case "trackables":
                        //var realresult = Repository.Where<Trackable>(request.Predicate);
                        var uniqueR = relationships.Select(x => x.TrackableName).Distinct().ToList();
                        foreach (string tn in uniqueR)
                        {
                            result.Add(Repository.Find<Trackable>(tn));
                        }
						//relationships.ForEach(r => { result.Add(Repository.Find<Trackable>(r.TrackableName)); });
						break;
					case "transactions":
						relationships.ForEach(r => { result.Add(Repository.Find<Transaction>(r.TransactionName)); });
						break;
                    case "trackablesenh":
                        var uniqueR2 = relationships.Select(x => x.TrackableName).Distinct().ToList();
                        foreach (string tn in uniqueR2)
                        {
                            var trackable = Repository.Find<Trackable>(tn);
                           // var trackableSearchResult = Augment(trackable, relationships);
                            var trackableSearchResult = Augment(trackable);
                            result.Add(trackableSearchResult);
                        }
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
		
		

        private TrackableSearchResult Augment(Trackable trackable, List<Relationship> relationships)
        {
            throw new NotImplementedException("can not assume the relationships being passed in have all the one's in need");
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

        private TrackableSearchResult Augment(Trackable trackable)
        {
            TrackableSearchResult result = new TrackableSearchResult(trackable);
            //result.Locations = Locate(result.Name);
            var relationships = Repository.Where(r => r.TrackableName == result.Name && r.Active == true);// && (r.Type == Database.Entity.TransactionType.move || r.Type == Database.Entity.TransactionType.copy));
            foreach (Relationship rel in relationships)
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
