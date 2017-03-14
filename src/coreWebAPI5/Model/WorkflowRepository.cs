using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace workflow.Model
{
	public class WorkflowRepository : IWorkflowRepository
	{
		private static ConcurrentDictionary<string, Workflow> _Workflow =
			new ConcurrentDictionary<string, Workflow>();
		private static ConcurrentDictionary<string, Trackable> _Trackable =
				 new ConcurrentDictionary<string, Trackable>();
		private static ConcurrentDictionary<string, ExecutedMove> _ExecutedMove =
				 new ConcurrentDictionary<string, ExecutedMove>();
		private static ConcurrentDictionary<string, Transaction> _Transaction =
				 new ConcurrentDictionary<string, Transaction>();
		public WorkflowRepository()
		{
		}

		#region Generic Methods
		private void Add<T>(T item) where T : BaseWorkflowItem
		{
			if (item == null)
				return;
			using (var db = new WorkflowContext())
			{
				try
				{
					/// HACK HACK HACK - should be using NoSql or create relational model for all items.

					//save first to get DB created PK
					BaseWorkflowItem saveThis = new BaseWorkflowItem();
					saveThis.DerivedType = typeof(T).ToString();
					db.WorkflowDb.Add(saveThis);
					db.SaveChanges();
					// now update with the serialized version so the workflowItemId is in the JSON
					item.Key = saveThis.Key;
					saveThis.SerializedObject = saveThis.Serialize<T>(item);
					db.WorkflowDb.Update(saveThis);
					int recordCount = db.SaveChanges();
					Console.WriteLine("Saved {0} records to DB", recordCount);

					Console.WriteLine("Primary WFKey is {0} ", saveThis.Key);
				}
				catch (Exception ex)
				{
					Console.WriteLine("{0} error", ex.Message);
					Console.WriteLine("{0} inner message", ex.InnerException);
				}
			}

		}

		public IEnumerable<T> GetAll<T>()
		{
			using (var db = new WorkflowContext())
			{
				try
				{
					Console.WriteLine("trying to return all {0} from DB", typeof(T).ToString());
					var wfi = db.WorkflowDb.Where(i => i.DerivedType == typeof(T).ToString());
					List<T> wf = new List<T>();
					foreach (var item in wfi)
						wf.Add(item.Deserialize<T>(item.SerializedObject));
					Console.WriteLine("found {0} items from DB", wf.Count.ToString());
					return wf;
				}
				catch (Exception ex)
				{
					Console.WriteLine("{0} error", ex.Message);
					Console.WriteLine("{0} inner message", ex.InnerException);
					return null;
				}
			}
		}
		public T Find<T>(int workflowItemId)
		{
			using (var db = new WorkflowContext())
				try {
					Console.WriteLine("searching for item {0} with Id {1}", typeof(T).ToString(), workflowItemId);
					BaseWorkflowItem result = db.WorkflowDb.Find(new object[] { workflowItemId });
					if (result == null)
						throw new WorkFlowException(String.Format("null was returned when finding for key {0}", workflowItemId));
					Console.WriteLine("found item");
					return result.Deserialize<T>(result.SerializedObject);

				} catch (Exception ex) {
					Console.WriteLine("{0} error", ex.Message);
					Console.WriteLine("{0} inner message", ex.InnerException);
					return default(T);
				}
		}
		public void Update<T>(T item) where T : BaseWorkflowItem
		{
			using (var db = new WorkflowContext())
			{
				try
				{
					Console.WriteLine("trying to update {0} itemId", item.Key);
					db.WorkflowDb.Update(item);
					db.SaveChanges();
					Console.WriteLine("ItemId {0} updated in database");
				}
				catch (Exception ex)
				{
					Console.WriteLine("{0} error", ex.Message);
					Console.WriteLine("{0} inner message", ex.InnerException);
				}
			}
		}
		public void Remove<T>(int workflowItemId) where T:BaseWorkflowItem
		{
				using (var db = new WorkflowContext()) { 
					try
					{
					var delete = Find<T>(workflowItemId);
					Console.WriteLine("trying to delete {0} itemId", workflowItemId);
					db.WorkflowDb.Remove(delete);
					db.SaveChanges();
					Console.WriteLine("ItemId {0} deleted from database");
					}
					catch (Exception ex)
					{
						Console.WriteLine("{0} error", ex.Message);
						Console.WriteLine("{0} inner message", ex.InnerException);
					}
				}
		}

		#endregion
		public void Add(Workflow workflow)
		{
			Add<Workflow>(workflow);
		}

		public void Add(Trackable trackable)
		{
			Add<Trackable>(trackable);
		}

		public void Add(Transaction trans)
		{
			Add<Transaction>(trans);
		}

		public bool CheckValidUserKey(string stringValue)
		{
			var userkeylist = new List<string>();
			userkeylist.Add("eric");
			userkeylist.Add("test");
			if(userkeylist.Contains(stringValue))
				return true;
			return false;
		}
		
	}
}
