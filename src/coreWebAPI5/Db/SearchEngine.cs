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
			return Repository.Find<T>(q);
			

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
