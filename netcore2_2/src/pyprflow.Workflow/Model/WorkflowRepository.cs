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
//using pyprflow.Db;
using pyprflow.Database;
using pyprflow.DbEntity;

namespace pyprflow.Workflow.Model
{
  
	public class WorkflowRepository : IWorkflowRepository
	{
        internal readonly DbContextOptions<ApiContext> _options;
        
        public WorkflowRepository(DbContextOptions<ApiContext> options)
        {
             _options = options; 
        }

        #region Generic Methods
        public void Add<T>(T item) where T : BaseWorkflowItem
        {
            if (item == null)
                return;
            using (var db = new ApiContext(_options))
            {
                try
                {
                    //Console.WriteLine("saving {0} with type {1}", item.Name, item.DerivedType);
                    Helpers.ObjectConverter converter = new Helpers.ObjectConverter();
                    BaseWorkflowItem saveThis = converter.GetBase<T>(item);
                    pyprflow.DbEntity.BaseDbWorkFlowItem dbSaveThis =
                        new Helpers.ObjectConverter().Map(saveThis);
                        
                    db.WorkflowDb.Add(dbSaveThis);
                       
                    // db.WorkflowDb.Add(saveThis);
                    int recordCount = db.SaveChanges();
                    if (item is pyprflow.Workflow.Model.Transaction)
                        AddRelationship(item as Transaction);
                    else
                        AddTransaction(item);
                    //Console.WriteLine("implement tracking / relationship saving here");

                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0} error", ex.Message);
                    Console.WriteLine("{0} inner message", ex.InnerException);
                }
                finally{
                    db.Dispose();
                }
            }
        }
       
        public void Update<T>(T item) where T : BaseWorkflowItem
        {
            using (var db = new ApiContext(_options))
            {
                try
                {
                    item.Active = false;
                    Helpers.ObjectConverter converter = new Helpers.ObjectConverter();
                    BaseWorkflowItem updateThis = converter.GetBase<T>(item);

                    pyprflow.DbEntity.BaseDbWorkFlowItem dbUpdateThis =
                        new Helpers.ObjectConverter().Map(updateThis);
                            
                            

                    //  BaseWorkflowItem updateThis = item.GetBase<T>(item);

                    //db.WorkflowDb.Update(updateThis);
                    db.WorkflowDb.Update(dbUpdateThis);
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
       
        public IEnumerable<T> GetAll<T>() where T : BaseWorkflowItem
		{
            Type providedtype = typeof(T);
            if (providedtype.GetTypeInfo().BaseType == typeof(BaseWorkflowItem))
            {
                using (var db = new ApiContext(_options))
                {
                    try
                    {
                        //	Console.WriteLine("trying to return all {0} from DB", typeof(T).ToString());
                        var wfi = db.WorkflowDb.Where(i => i.DerivedType == typeof(T).ToString() && i.Deleted==false);
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
		
        public bool Exist<T>(string itemName) where T : BaseWorkflowItem
        {
            bool result = false;
            using (var db = new ApiContext(_options))
            {
                try
                {
                    result = db.WorkflowDb.Any(i =>
                    i.Name == itemName && i.Active == true );
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return result;
        }

        public BaseWorkflowItem Find<T>(string workflowName, Type returnType)  
        {
            var result = FindDBItem(workflowName, returnType,false);
            if (result == null)
                return null;
            // return result ;
            if (returnType.ToString() != result.DerivedType)
                throw new WorkFlowException("type of object returned does not match type requested");
            return result.Deserialize(result.SerializedObject, returnType) as BaseWorkflowItem;
          //  return result.Deserialize<T>(result.SerializedObject);

        }

        public T Find<T>(string workflowName) where T : BaseWorkflowItem
        {
            var result = FindDBItem<T>(workflowName, false);
            if (result == null)
                return default(T);
            return result.Deserialize<T>(result.SerializedObject);
                
	    }
	   
        public void SoftDelete<T>(string workflowItemId) where T : BaseWorkflowItem
        {
                try
                {
                    var deactivate = Find<T>(workflowItemId);
                    deactivate.Deleted = true;
                    Update<T>(deactivate);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0} error", ex.Message);
                    Console.WriteLine("{0} inner message", ex.InnerException);
                }
        }

		public void HardDelete<T>(string workflowItemId) where T:BaseWorkflowItem
		    {
            throw new NotImplementedException("needs to be tested");
            Type providedtype = typeof(T);
            if (providedtype.GetTypeInfo().BaseType == typeof(BaseWorkflowItem))
            {
                using (var db = new ApiContext(_options))
                {
                    try
                    {

                        var delete = new BaseWorkflowItem();
                        delete.Name = workflowItemId;
                        delete.DerivedType = typeof(T).ToString();

                        pyprflow.DbEntity.BaseDbWorkFlowItem dbDelete =
                            new Helpers.ObjectConverter().Map(delete);

                        db.WorkflowDb.Remove(dbDelete);

                      //  db.WorkflowDb.Remove(delete);
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
        public List<BaseWorkflowItem> Where<T>(System.Linq.Expressions.Expression<Func<pyprflow.DbEntity.Relationship, bool>> predicate, Type returnType) where T : BaseWorkflowItem
        {
            var rel = Where(predicate);  // query the Relationship table
            if (rel.Count == 0)
                return null;// GetAll<T>().ToList();
            List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();
            switch (returnType.ToString().ToLower()) // get the object by name, take the first out of the relationship table 
                                                    //  query the relationship table just to get the name of the object i'm looking for.
                                                   // ToDo: deserialize to the correct type instead of T
            {
                case "pyprflow.workflow.model.workflow":
                    foreach (Relationship r in rel.GroupBy(o => o.WorkflowName).Select(g => g.First()))
                        result.Add(this.Find<T>(r.WorkflowName, returnType));
                    break;
                case "pyprflow.workflow.model.trackable":
                    foreach (Relationship r in rel.GroupBy(o => o.TrackableName).Select(g => g.First()))
                        result.Add(this.Find<T>(r.TrackableName, returnType));
                    break;
                case "pyprflow.workflow.model.transaction":
                    foreach (Relationship r in rel.GroupBy(o => o.TransactionName).Select(g => g.First()))
                        result.Add(this.Find<T>(r.TransactionName, returnType));
                    break;
                
            }
            return result;

        }

        public List<T> Where<T>(System.Linq.Expressions.Expression<Func<pyprflow.DbEntity.Relationship, bool>> predicate) where T :BaseWorkflowItem
        {
            var rel = Where(predicate);  // query the Relationship table
            if (rel.Count == 0)
                return null;// GetAll<T>().ToList();
            List<T> result = new List<T>();
            switch(typeof(T).ToString().ToLower()) // get the object by name, take the first out of the relationship table 
                //  query the relationship table just to get the name of the object i'm looking for.
            {
                case "pyprflow.workflow.model.workflow":
                    foreach (Relationship r in rel.GroupBy(o => o.WorkflowName).Select(g => g.First()))
                        result.Add(this.Find<T>(r.WorkflowName));
                    break;
                case "pyprflow.workflow.model.trackable":
                    foreach (Relationship r in rel.GroupBy(o => o.TrackableName).Select(g => g.First()))
                        result.Add(this.Find<T>(r.TrackableName));
                    break;
                case "pyprflow.workflow.model.transaction":
                    foreach (Relationship r in rel.GroupBy(o => o.TransactionName).Select(g => g.First()))
                        result.Add(this.Find<T>(r.TransactionName));
                    break;
                default:
                    foreach (Relationship r in rel.GroupBy(o => o.WorkflowName).Select(g => g.First()))
                        if (!String.IsNullOrEmpty(r.WorkflowName))
                            result.Add(this.Find<T>(r.WorkflowName));
                    foreach (Relationship r in rel.GroupBy(o => o.TrackableName).Select(g => g.First()))
                        if (!String.IsNullOrEmpty(r.TrackableName))
                            result.Add(this.Find<T>(r.TrackableName));
                    foreach (Relationship r in rel.GroupBy(o => o.TransactionName).Select(g => g.First()))
                        if (!String.IsNullOrEmpty(r.TransactionName))
                            result.Add(this.Find<T>(r.TransactionName));

                    break;

            }


            return result;

        }

        public List<Relationship> Where(System.Linq.Expressions.Expression<Func<pyprflow.DbEntity.Relationship, bool>> predicate)
        {
            using (var db = new ApiContext(_options))
            {
                List<pyprflow.DbEntity.Relationship> dbList = db.Relationships.Where(predicate.Compile()).ToList();
                return new Helpers.ObjectConverter().Map(dbList);
            }
        }

        public bool Execute(Transaction trans, out int statusCode, out string msg)
        {
            return trans.Execute(this, out statusCode, out msg);
            //return true;
        }
        //      public List<Relationship> GetAll(System.Linq.Expressions.Expression<Func<pyprflow.DbEntity.Relationship, bool>> predicate)
        //{
        //	using (var db = new ApiContext(_options))
        //	{
        //             // throw new NotImplementedException("refactor for mapping");

        //              var dbList = db.Relationships.Where(predicate).ToList();
        //              return new Helpers.ObjectConverter().Map(dbList);

        //	}
        //}


        #region Private Methods to Manage the Relationships


        private void AddTransaction<T>(T item) where T : BaseWorkflowItem
        {
            Helpers.ObjectConverter converter = new Helpers.ObjectConverter();
            pyprflow.DbEntity.Relationship r = new DbEntity.Relationship();
            using (var db = new ApiContext(_options))
            {
                if (item is Workflow)
                    r = converter.Create(item as Workflow);
                if (item is Trackable)
                    r = converter.Create(item as Trackable);
                db.Relationships.Add(r);
                db.SaveChanges();
            }
        }

        private pyprflow.DbEntity.BaseDbWorkFlowItem FindDBItem<T>(string workflowItemName, bool deleted) where T : BaseWorkflowItem
        {
            using (var db = new ApiContext(_options))
            {
                try
                {
                    Console.WriteLine("searching for item {0} with Id {1}", typeof(T).ToString(), workflowItemName);

                    //  BaseWorkflowItem result = db.WorkflowDb.Find(new object[] { workflowName, typeof(T).ToString() });
                    pyprflow.DbEntity.BaseDbWorkFlowItem result
                        = db.WorkflowDb.Find(new object[] { workflowItemName, typeof(T).ToString() });

                    if (result.Deleted != deleted)
                        result = null; // hack hack hack.. 

                    if (result == null)
                    {
                        Console.WriteLine("looking for type {0} with ID {1}", typeof(T).ToString(), workflowItemName);
                        throw new WorkFlowException(String.Format("null was returned when finding for key {0}", workflowItemName));
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("looking for type {0} with ID {1}", typeof(T).ToString(), workflowItemName);
                    return null;
                    //   throw new WorkFlowException(String.Format("null was returned when finding for key {0}", workflowItemName));
                }
            }
        }

        private pyprflow.DbEntity.BaseDbWorkFlowItem FindDBItem(string workflowItemName, Type returnType, bool deleted)  
        {
            using (var db = new ApiContext(_options))
            {
                try
                {
                    Console.WriteLine("searching for item {0} with Id {1}", returnType, workflowItemName);

                    //  BaseWorkflowItem result = db.WorkflowDb.Find(new object[] { workflowName, typeof(T).ToString() });
                    pyprflow.DbEntity.BaseDbWorkFlowItem result
                        = db.WorkflowDb.Find(new object[] { workflowItemName, returnType.ToString() });

                    if (result.Deleted != deleted)
                        result = null; // hack hack hack.. 

                    if (result == null)
                    {
                        Console.WriteLine("looking for type {0} with ID {1}", typeof(BaseWorkflowItem).ToString(), workflowItemName);
                        throw new WorkFlowException(String.Format("null was returned when finding for key {0}", workflowItemName));
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("looking for type {0} with ID {1}", typeof(BaseWorkflowItem).ToString(), workflowItemName);
                    return null;
                    //   throw new WorkFlowException(String.Format("null was returned when finding for key {0}", workflowItemName));
                }
            }
        }

        /// <summary>
        /// Adds the Relationship record to DB
        /// </summary>
        /// <param name="trans"></param>
        private void AddRelationship(Transaction trans)
        {
            Console.WriteLine("tracking methods");
            try
            {
                Helpers.ObjectConverter converter = new Helpers.ObjectConverter();
                var r = converter.CreateRelationshipObj(trans);
                Console.WriteLine("transacation type is {0}", trans.type);
                if (trans.type == TransactionType.move)
                    DeActivateOldTrackableRelationship(trans);
                // Add<Relationship>(r);
                InsertRelationship(r);
            }
            catch (Exception ex)
            {
                Console.WriteLine("and error occured {0} stack {1}", ex.Message, ex.StackTrace);
            }

        }

        private void DeActivateOldTrackableRelationship(Transaction r)
		{
            if (r.type != TransactionType.move)
                throw new InvalidOperationException("Deactiving old relationships is only valid for Move Transaction Types");
			using (var db = new ApiContext(_options))
			{
                Console.WriteLine("looking for old relationships");
               
                List<pyprflow.DbEntity.Relationship> oldr = 
                    db.Relationships.Where(o => o.TrackableName == r.TrackableName
                    && o.WorkflowName == r.WorkflowName
                    && o.NodeName == r.CurrentNodeId).ToList();

                Console.WriteLine("looking for {0} in WF {1}, with nodeID = {2}", r.TrackableName, r.WorkflowName, r.CurrentNodeId);
                if (oldr == null)
                {
                    Console.WriteLine("didn't find an old relationship");
                    return;// null;
                }
                Console.WriteLine("found {0} relationships", oldr.Count);
                foreach (pyprflow.DbEntity.Relationship relationship in oldr)
                {
                    relationship.Active = false;
                    DeactivateWorkflowItem<Transaction>(relationship.TransactionName);
                    db.Relationships.Update(relationship);
                }
                db.SaveChanges();
                return;// oldr;
			}
		}

        private void DeactivateWorkflowItem<T>(string transactionName) where T : BaseWorkflowItem
        {
          
                var t = Find<T>(transactionName);
                t.Active = false;
                Update<T>(t);

        }

        private void InsertRelationship(Relationship r)
		{
            
            using (var db = new ApiContext(_options))
            {
                try
                {
                    pyprflow.DbEntity.Relationship dbR =
                        new Helpers.ObjectConverter().Map(r);

                    db.Relationships.Add(dbR);
                    //db.Relationships.Add(r);
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

        #endregion

        public void EmptyAll()
        {
            using (var db = new ApiContext(_options))
            {

                try
                {
                    if (db.Database.GetDbConnection().GetType().Name == "SqliteConnection")
                    {
                        db.Database.ExecuteSqlCommand("delete from relationships");
                        db.Database.ExecuteSqlCommand("delete from workflowDb");
                        db.Database.ExecuteSqlCommand("vacuum");
                    }
                    if (db.Database.GetDbConnection().GetType().Name == "NpgsqlConnection")
                    {
                        db.Database.ExecuteSqlCommand("delete from public.\"Relationships\"");
                        db.Database.ExecuteSqlCommand("Delete FROM public.\"WorkflowDb\"");
                      
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("truncate table Relationships");
                        db.Database.ExecuteSqlCommand("truncate table workflowDb");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0} error", ex.Message);
                    Console.WriteLine("{0} inner message", ex.InnerException);
                }
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
