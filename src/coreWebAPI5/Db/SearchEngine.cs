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

		

	
    }
}
