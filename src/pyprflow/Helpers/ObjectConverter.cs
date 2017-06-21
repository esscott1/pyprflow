using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pyprflow.Model;

namespace pyprflow.Helpers
{
    public class ObjectConverter
    {
        public BaseWorkflowItem GetBase<T>(T derivedObject) where T : BaseWorkflowItem
        {
            BaseWorkflowItem result = new BaseWorkflowItem();
            result.Name = derivedObject.Name;
            result.Active = derivedObject.Active;
            result.SerializedObject = derivedObject.Serialize<T>(derivedObject);
            result.DerivedType = typeof(T).ToString();
            return result;
        }

        public pyprflow.Database.Entity.BaseWorkflowItem Map(pyprflow.Model.BaseWorkflowItem item)
        {
            pyprflow.Database.Entity.BaseWorkflowItem result =
                new pyprflow.Database.Entity.BaseWorkflowItem();
            result.Active = item.Active;
            result.DerivedType = item.DerivedType;
            result.Name = item.Name;
            result.SerializedObject = item.SerializedObject;
            return result;
        }

        public pyprflow.Model.BaseWorkflowItem Map(pyprflow.Database.Entity.BaseWorkflowItem item)
        {
            pyprflow.Model.BaseWorkflowItem result =
                new pyprflow.Model.BaseWorkflowItem();
            result.Active = item.Active;
            result.DerivedType = item.DerivedType;
            result.Name = item.Name;
            result.SerializedObject = item.SerializedObject;
            return result;
        }

        public pyprflow.Database.Entity.Relationship Map(pyprflow.Model.Relationship item)
        {
            pyprflow.Database.Entity.Relationship result =
                new Database.Entity.Relationship();
            result.Active = item.Active;
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
        public pyprflow.Model.Relationship Map(pyprflow.Database.Entity.Relationship item)
        {
            pyprflow.Model.Relationship result =
                new pyprflow.Model.Relationship();
            result.Active = item.Active;
            result.AssignedTo = item.AssignedTo;
            result.NodeName = item.NodeName;
            result.RelationshipId = item.RelationshipId;
            result.Submitter = item.Submitter;
            result.TimeStamp = item.TimeStamp;
            result.TrackableName = item.TrackableName;
            result.TransactionName = item.TransactionName;

            int iType = (int)item.Type;
            result.Type = (pyprflow.Model.TransactionType)iType;

            result.WorkflowName = item.WorkflowName;
            return result;

        }

        public List<pyprflow.Model.Relationship> Map(List<pyprflow.Database.Entity.Relationship> items)
        {
            List<pyprflow.Model.Relationship> result = new List<Relationship>();
            foreach (pyprflow.Database.Entity.Relationship item in items)
            {
                result.Add(Map(item));
            }
            return result;
        }
    }
}
