using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
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


		

		//public List<T> SearchAll<T>(Dictionary<string,
		//	Microsoft.Extensions.Primitives.StringValues> values) where T : BaseWorkflowItem
		//{
		//	List<T> result = new List<T>();

		//	string w = values.FirstOrDefault(v => v.Key.ToLower() == "where").Value;
		//	var kvp = w.Split('=');
		//	string col = kvp[0];
		//	string val = kvp[1];
		//	switch (kvp.Key)
		//	{
		//		case "workflowid":
		//			Repository.Where<Workflow>(j => j.WorkflowName == kvp[0]);
		//			break;
		//		case "trackableid":
		//			Repository.Where<Workflow>(j => j. == kvp[0]);
		//			break;
		//		case "transactionid":
		//			break;
		//		case "nodeid":
		//			break;
		//		case "start":
		//			break;
		//		case "end":
		//			break;

		//	}
			
		//	return result;
		//}


		public List<BaseWorkflowItem> Search(Dictionary<string,
			Microsoft.Extensions.Primitives.StringValues> values)
		{
			// select the Relationships that are determined by the where clause
			//  find in Relationships based on where clause return list of strings 
			// which are IDs of objects that are to be returned
			List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();
			Console.WriteLine("in the search engine with {0} value count", values.Count);
			StringValues wvalue;
			values.TryGetValue("where", out wvalue);
			string w = wvalue.ToString();
			//string w = values.FirstOrDefault(v => v.Key.ToLower() == "where").Value;
			string[] aw = w.Split('=');

			Console.WriteLine("string count {0} ", aw.Count());
			List<Relationship> relationships = new List<Relationship>();
			// finding the relationships based on teh where clause provided
			switch (aw[0].ToLower())
			{
				case "workflowid":
					relationships = Repository.Where(r => r.WorkflowName == aw[1]);
					break;
				case "trackableid":
					relationships = Repository.Where(r => r.TrackableName == aw[1]);
					break;
				case "transactionid":
					relationships = Repository.Where(r => r.TransactionName == aw[1]);
					break;
				case "nodeid":
					relationships = Repository.Where(r => r.NodeName == aw[1]);
					break;
				default:
					Console.WriteLine("{0} is not a valid WHERE keyword", aw[0]);
					return null;
					break;
			}
			Console.WriteLine("found {0} relationships ", relationships.Count);
			//  find the return objects based on what was selected for.
			string select = values.FirstOrDefault(v => v.Key.ToLower() == "select").Value;
				switch (select.ToLower())
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
						Console.WriteLine("{0} is not a valid SELECT keyword", select);
						return null;
						break;
			}

			return result;

		}







	}
}
