using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using pyprflow.Db;


namespace pyprflow.Model
{
	public class WorkflowRepository : IWorkflowRepository
	{
        internal readonly DbContextOptions<WorkflowContext> _options;

        public WorkflowRepository(DbContextOptions<WorkflowContext> options)
        {
            _options = options;
        }

        #region Generic Methods
        public void Add<T>(T item) where T : BaseWorkflowItem
        {
            if (item == null)
                return;
            if (item is pyprflow.Model.BaseWorkflowItem)
            {
                using (var db = new WorkflowContext(_options))
                {
                    try
                    {
                        //Console.WriteLine("saving {0} with type {1}", item.Name, item.DerivedType);
                        Helpers.ObjectConverter converter = new Helpers.ObjectConverter();
                        BaseWorkflowItem saveThis = converter.GetBase<T>(item);
                       // BaseWorkflowItem saveThis = item.GetBase<T>(item);

                        db.WorkflowDb.Add(saveThis);
                        int recordCount = db.SaveChanges();
                        if (item is pyprflow.Model.Transaction)
                            Track(item as Transaction);
                        //Console.WriteLine("implement tracking / relationship saving here");

                        //Console.WriteLine("Saved {0} records to DB", recordCount);

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


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0} error", ex.Message);
                        Console.WriteLine("{0} inner message", ex.InnerException);
                    }
                }
            }
            else if (item is pyprflow.Model.Relationship)
            {
                using (var db = new WorkflowContext(_options))
                {
                    try
                    {


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0} error", ex.Message);
                        Console.WriteLine("{0} inner message", ex.InnerException);
                    }


                }
            }
        }

		public IEnumerable<T> GetAll<T>() where T : BaseWorkflowItem
		{
            Type providedtype = typeof(T);
            if (providedtype.GetTypeInfo().BaseType == typeof(BaseWorkflowItem))
            {
                using (var db = new WorkflowContext(_options))
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
            else return null;
		}
		
	    public T Find<T>(string workflowName) where T : BaseWorkflowItem
        {
            Type providedtype = typeof(T);
            if (providedtype.GetTypeInfo().BaseType == typeof(BaseWorkflowItem))
            {
                using (var db = new WorkflowContext(_options))
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
            else return default(T);
	    }
	    public void Update<T>(T item) where T : BaseWorkflowItem
		{
            if (item is pyprflow.Model.BaseWorkflowItem)
            {
                using (var db = new WorkflowContext(_options))
                {
                    try
                    {
                        item.Active = false;
                        Helpers.ObjectConverter converter = new Helpers.ObjectConverter();
                        BaseWorkflowItem updateThis = converter.GetBase<T>(item);
                      //  BaseWorkflowItem updateThis = item.GetBase<T>(item);

                        db.WorkflowDb.Update(updateThis);
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
		}
        public void Deactivate<T>(string workflowItemId) where T : BaseWorkflowItem
        {
            Type providedtype = typeof(T);
            if (providedtype.GetTypeInfo().BaseType == typeof(BaseWorkflowItem))
            {
                try
                {
                    var deactivate = Find<T>(workflowItemId);
                    deactivate.Active = false;
                    Update<T>(deactivate);
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
            Type providedtype = typeof(T);
            if (providedtype.GetTypeInfo().BaseType == typeof(BaseWorkflowItem))
            {
                using (var db = new WorkflowContext(_options))
                {
                    try
                    {

                        var delete = new BaseWorkflowItem();
                        delete.Name = workflowItemId;
                        delete.DerivedType = typeof(T).ToString();

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
		    }

		#endregion
        public void EmptyAll()
        {
            using (var db = new WorkflowContext(_options))
            {
              
                try
                {
                    db.Database.ExecuteSqlCommand("truncate table Relationships");
                    db.Database.ExecuteSqlCommand("truncate table workflowDb");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0} error", ex.Message);
                    Console.WriteLine("{0} inner message", ex.InnerException);
                }
            }

        }
       
		/// <summary>
		/// Adds the Relationship record to DB
		/// </summary>
		/// <param name="trans"></param>
		private void Track(Transaction trans)
		{
			Console.WriteLine("tracking methods");
			try
			{
                
                var r = trans.CreateRelationshipObj();
				Console.WriteLine("transacation type is {0}", trans.type);
				if (trans.type == TransactionType.move)
					DeActivateOldTrackableRelationship(trans);
               // Add<Relationship>(r);
				InsertRelationship(r);
			}
			catch(Exception ex)
			{
				Console.WriteLine("and error occured {0} stack {1}",ex.Message, ex.StackTrace);
			}

		}
		public List<Relationship> GetAll(System.Linq.Expressions.Expression<Func<Relationship, bool>> predicate)
		{
			using (var db = new WorkflowContext(_options))
			{
				return db.Relationships.Where(predicate).ToList();

			}
		}

		private void DeActivateOldTrackableRelationship(Transaction r)
		{
			using (var db = new WorkflowContext(_options))
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

		private void InsertRelationship(Relationship r)
		{
            
            using (var db = new WorkflowContext(_options))
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
			using (var db = new WorkflowContext(_options))
			{
				//Console.WriteLine("in the Where method of WorkflowRepository");
				return db.Relationships.Where(predicate.Compile()).ToList();
			}
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
