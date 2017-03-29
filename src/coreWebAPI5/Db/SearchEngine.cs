using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using workflow.Model;

namespace workflow.Db
{
    public class SearchEngine
    {
		private IWorkflowRepository Repository { get; set; }
		public SearchEngine(IWorkflowRepository repository)
		{
			Repository = repository;
		}

		public T Search<T>(Dictionary<string, 
			Microsoft.Extensions.Primitives.StringValues> values) where T : BaseWorkflowItem
		{
			string q = values.FirstOrDefault(v => v.Key == "id").Value;
			if(q!=null)
				return Repository.Find<T>(q);
			return null;
			

		}

		public List<T> SearchAll<T>(Dictionary<string,
			Microsoft.Extensions.Primitives.StringValues> values) where T : BaseWorkflowItem
		{

			List<T> result = new List<T>();
			string w = values.FirstOrDefault(v => v.Key.ToLower() == "nodeid").Value;
			var relationships = Repository.Where(r => r.NodeName == w);
			foreach (Relationship r in relationships)
			{
				result.Add(Repository.Find<T>(r.TrackableName));

			}

			return result;

		}

		public List<T> SearchAll<T>(string clause)
		{
			// parse the clause for conditional statements
			//clause.Split("=")
			//	.Select((v, i) => new { v, i })
			//	.GroupBy(x => x.i / 2)
			//	.ToDictionary(g => g.First().v,
			//	g => g.Last().v);
			return null;
		}

		public BaseWorkflowItem Search(Dictionary<string,
			Microsoft.Extensions.Primitives.StringValues> values)
		{
			string q = values.FirstOrDefault(v => v.Key == "q").Value;
			BaseWorkflowItem result = null;
			switch (q.ToLower())
			{
				case "workflow":
					result = Search<Workflow>(values);
					break;
				case "trackable":
					result = Search<Trackable>(values);
					break;
				case "node":
					break;
				case "transaction":
					result = Search<Transaction>(values);
					break;
			}
			return result;

		}







	}
}
