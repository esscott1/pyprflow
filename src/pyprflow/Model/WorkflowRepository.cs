﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace pyprflow.Model
{
	public class WorkflowRepository : IWorkflowRepository
	{
		private static ConcurrentDictionary<string, Workflow> _Workflow =
			new ConcurrentDictionary<string, Workflow>();
		private static ConcurrentDictionary<string, Trackable> _Trackable =
				 new ConcurrentDictionary<string, Trackable>();
		
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
					//Console.WriteLine("saving {0} with type {1}", item.Name, item.DerivedType);
					BaseWorkflowItem saveThis = new BaseWorkflowItem();
					saveThis.SerializedObject = saveThis.Serialize<T>(item);
					saveThis.DerivedType = typeof(T).ToString();
					saveThis.Name = item.Name;
					db.WorkflowDb.Add(saveThis);
					
					int recordCount = db.SaveChanges();
					
					//Console.WriteLine("Saved {0} records to DB", recordCount);

				}
				catch(Microsoft.Data.Sqlite.SqliteException ex)
				{
					if (ex.SqliteErrorCode == 19)
						throw new WorkFlowException("unique key violation");
					if (ex.SqliteErrorCode == 1)
					{
						Console.WriteLine("need to run a migration, table does not exist");
						Console.WriteLine("exception {0}", ex.Message);
					}
					Console.WriteLine("sqlite code {0}", ex.SqliteErrorCode.ToString());

				
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
				//	Console.WriteLine("trying to return all {0} from DB", typeof(T).ToString());
					var wfi = db.WorkflowDb.Where(i => i.DerivedType == typeof(T).ToString());
					List<T> wf = new List<T>();
					foreach (var item in wfi)
						wf.Add(item.Deserialize<T>(item.SerializedObject));
				//	Console.WriteLine("found {0} items from DB", wf.Count.ToString());
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
		
	public T Find<T>(string workflowName)
	{
		using (var db = new WorkflowContext())
		{
			try
			{
				Console.WriteLine("searching for item {0} with Id {1}", typeof(T).ToString(), workflowName);
				BaseWorkflowItem result = db.WorkflowDb.Find(new object[] { workflowName, typeof(T).ToString() });
				if (result == null)
				{
					Console.WriteLine("looking for type {0} with ID {1}", typeof(T).ToString(), workflowName);
					throw new WorkFlowException(String.Format("null was returned when finding for key {0}", workflowName));
				}
				//	Console.WriteLine("found item");
				return result.Deserialize<T>(result.SerializedObject);

			}
			catch (Microsoft.Data.Sqlite.SqliteException ex)
			{
				if (ex.SqliteErrorCode == 19)
					throw new WorkFlowException("unique key violation");
				if (ex.SqliteErrorCode == 1)
				{
					Console.WriteLine("need to run a migration, table does not exist");
					Console.WriteLine("exception {0}", ex.Message);
				}
				Console.WriteLine("sqlite code {0}", ex.SqliteErrorCode.ToString());
				return default(T);

			}
			catch (Exception ex)
			{
				Console.WriteLine("{0} error", ex.Message);
				Console.WriteLine("{0} inner message", ex.InnerException);
				return default(T);
			}
		}
	}
	public void Update<T>(T item) where T : BaseWorkflowItem
		{
			using (var db = new WorkflowContext())
			{
				try
				{
				//	Console.WriteLine("trying to update {0} itemId", item.Name);
					db.WorkflowDb.Update(item);
					db.SaveChanges();
				//	Console.WriteLine("ItemId {0} updated in database");
				}
				catch (Exception ex)
				{
					Console.WriteLine("{0} error", ex.Message);
					Console.WriteLine("{0} inner message", ex.InnerException);
				}
			}
		}
		public void Remove<T>(string workflowItemId) where T:BaseWorkflowItem
		{
				using (var db = new WorkflowContext()) { 
					try
					{
					var delete = Find<T>(workflowItemId);
				//	Console.WriteLine("trying to delete {0} itemId", workflowItemId);
					db.WorkflowDb.Remove(delete);
					db.SaveChanges();
				//	Console.WriteLine("ItemId {0} deleted from database");
					}
					catch (Exception ex)
					{
						Console.WriteLine("{0} error", ex.Message);
						Console.WriteLine("{0} inner message", ex.InnerException);
					}
				}
		}

		#endregion
		/// <summary>
		/// Adds the Relationship record to DB
		/// </summary>
		/// <param name="trans"></param>
		public void Track(Transaction trans)
		{
			Console.WriteLine("tracking methods");
			try
			{
				var r = new Relationship();
				r.TransactionName = trans.Name;
				r.TrackableName = trans.TrackableName;
				if (trans.type == TransactionType.move)
					r.NodeName = trans.NewNodeId;
				else if (trans.type == TransactionType.copy)
					r.NodeName = trans.NewNodeId;
				else if (trans.type == TransactionType.assignment)
					r.NodeName = trans.CurrentNodeId;
				else if (trans.type == TransactionType.comment)
					r.NodeName = trans.CurrentNodeId;
				r.WorkflowName = trans.WorkflowName;
				if(trans.AssignedTo !=null)
					r.AssignedTo = trans.AssignedTo.Email;
				r.Type = trans.type;
				if(trans.Submitter != null)
					r.Submitter = trans.Submitter.Email;
				Console.WriteLine("transacation type is {0}", trans.type);
				if (trans.type == TransactionType.move)
					DeActivateOldTrackableRelationship(trans);

				InsertRelationship(r);
			}
			catch(Exception ex)
			{
				Console.WriteLine("and error occured {0} stack {1}",ex.Message, ex.StackTrace);
			}

		}
		public List<Relationship> GetAll(System.Linq.Expressions.Expression<Func<Relationship, bool>> predicate)
		{
			using (var db = new WorkflowContext())
			{
				return db.Relationships.Where(predicate).ToList();

			}
		}

		private void DeActivateOldTrackableRelationship(Transaction r)
		{
			using (var db = new WorkflowContext())
			{
				Console.WriteLine("looking for old relationships");
				List<Relationship> oldr = db.Relationships.Where(o => o.TrackableName == r.TrackableName
				&& o.WorkflowName == r.WorkflowName
				//&& o.Type == r.type
				&& o.NodeName == r.CurrentNodeId).ToList();
				Console.WriteLine("looking for {0} in WF {1}, with nodeID = {2}", r.TrackableName, r.WorkflowName, r.CurrentNodeId);
				if (oldr == null)
				{
					Console.WriteLine("didn't find an old relationship");
					return;// null;
				}
				Console.WriteLine("found {0} relationships",oldr.Count);
				foreach (Relationship relationship in oldr)
				{
					relationship.Active = false;
					db.Relationships.Update(relationship);
				}
				db.SaveChanges();
				//Console.WriteLine("updated {0} records during deactivate old relationshops", db.SaveChanges());
				return;// oldr;
			}
		}

		private static void InsertRelationship(Relationship r)
		{
			using (var db = new WorkflowContext())
			{
				try
				{
					db.Relationships.Add(r);
					db.SaveChanges();
				//	Console.WriteLine("saved relationship {0}", r.RelationshipId);
				}
				catch (Exception ex)
				{
					Console.WriteLine("error saving to Relationships, msg: {0}", ex.Message);
					Console.WriteLine("inner exeception, msg: {0}", ex.InnerException);
				}

			}
		}
		
		public List<Relationship> Where(System.Linq.Expressions.Expression<Func<Relationship, bool>> predicate)
		{
			using (var db = new WorkflowContext())
			{
				//Console.WriteLine("in the Where method of WorkflowRepository");
				return db.Relationships.Where(predicate.Compile()).ToList();
			}
		}

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