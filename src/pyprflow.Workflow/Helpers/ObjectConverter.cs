using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pyprflow.Workflow.Model;


namespace pyprflow.Workflow.Helpers
{
    public class ObjectConverter
    {
        public BaseWorkflowItem GetBase<T>(T derivedObject) where T : BaseWorkflowItem
        {
            BaseWorkflowItem result = new BaseWorkflowItem();
            result.Name = derivedObject.Name;
            result.Active = derivedObject.Active;
            result.Deleted = derivedObject.Deleted;
            result.SerializedObject = derivedObject.Serialize<T>(derivedObject);
            result.DerivedType = typeof(T).ToString();
            return result;
        }

        public pyprflow.Database.Entity.BaseWorkflowItem Map(pyprflow.Workflow.Model.BaseWorkflowItem item)
        {
            pyprflow.Database.Entity.BaseWorkflowItem result =
                new pyprflow.Database.Entity.BaseWorkflowItem();
            result.Active = item.Active;
            result.Deleted = item.Deleted;
            result.DerivedType = item.DerivedType;
            result.Name = item.Name;
            result.SerializedObject = item.SerializedObject;
            return result;
        }

        public pyprflow.Workflow.Model.BaseWorkflowItem Map(pyprflow.Database.Entity.BaseWorkflowItem item)
        {
            pyprflow.Workflow.Model.BaseWorkflowItem result =
                new pyprflow.Workflow.Model.BaseWorkflowItem();
            result.Active = item.Active;
            result.Deleted = item.Deleted;
            result.DerivedType = item.DerivedType;
            result.Name = item.Name;
            result.SerializedObject = item.SerializedObject;
            return result;
        }

        public pyprflow.Database.Entity.Relationship Map(pyprflow.Workflow.Model.Relationship item)
        {
            pyprflow.Database.Entity.Relationship result =
                new Database.Entity.Relationship();
            result.Active = item.Active;
            result.Deleted = item.Deleted;
            result.AssignedTo = item.AssignedTo;
            result.NodeName = item.NodeName;
            result.RelationshipId = item.RelationshipId;
            result.Submitter = item.Submitter;
            result.TimeStamp = item.TimeStamp;
            result.TrackableName = item.TrackableName;
            result.TransactionName = item.TransactionName;

            int iType = (int)item.Type;
            result.Type = (pyprflow.Database.Entity.TransactionType)iType;
           
            result.WorkflowName = item.WorkflowName;
            return result;

        }
        public pyprflow.Workflow.Model.Relationship Map(pyprflow.Database.Entity.Relationship item)
        {
            pyprflow.Workflow.Model.Relationship result =
                new pyprflow.Workflow.Model.Relationship();
            result.Active = item.Active;
            result.Deleted = item.Deleted;
            result.AssignedTo = item.AssignedTo;
            result.NodeName = item.NodeName;
            result.RelationshipId = item.RelationshipId;
            result.Submitter = item.Submitter;
            result.TimeStamp = item.TimeStamp;
            result.TrackableName = item.TrackableName;
            result.TransactionName = item.TransactionName;

            int iType = (int)item.Type;
            result.Type = (pyprflow.Workflow.Model.TransactionType)iType;

            result.WorkflowName = item.WorkflowName;
            return result;

        }

        public List<pyprflow.Workflow.Model.Relationship> Map(List<pyprflow.Database.Entity.Relationship> items)
        {
            List<pyprflow.Workflow.Model.Relationship> result = new List<Relationship>();
            foreach (pyprflow.Database.Entity.Relationship item in items)
            {
                result.Add(Map(item));
            }
            return result;
        }
    }
}
