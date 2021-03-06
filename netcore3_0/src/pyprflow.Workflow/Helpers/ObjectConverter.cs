﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pyprflow.Workflow.Model;


namespace pyprflow.Workflow.Helpers
{
    /// <summary>
    /// using this instead of AutoMapper due to limited mapping use and speed.  Future refactors may eliminate the need for mapping .
    /// </summary>
    internal class ObjectConverter
    {
        internal BaseWorkflowItem GetBase<T>(T derivedObject) where T : BaseWorkflowItem
        {
            BaseWorkflowItem result = new BaseWorkflowItem();
            result.Name = derivedObject.Name;
            result.Active = derivedObject.Active;
            result.Deleted = derivedObject.Deleted;
            result.SerializedObject = derivedObject.Serialize<T>(derivedObject);
            result.DerivedType = typeof(T).ToString();
            return result;
        }

        internal pyprflow.DbEntity.BaseDbWorkFlowItem Map(pyprflow.Workflow.Model.BaseWorkflowItem item)
        {
            pyprflow.DbEntity.BaseDbWorkFlowItem result =
                new pyprflow.DbEntity.BaseDbWorkFlowItem();
            result.Active = item.Active;
            result.Deleted = item.Deleted;
            result.DerivedType = item.DerivedType;
            result.Name = item.Name;
            result.SerializedObject = item.SerializedObject;
            return result;
        }

        internal pyprflow.Workflow.Model.BaseWorkflowItem Map(pyprflow.DbEntity.BaseDbWorkFlowItem item)
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

        internal pyprflow.DbEntity.Relationship Map(pyprflow.Workflow.Model.Relationship item)
        {
            pyprflow.DbEntity.Relationship result =
                new DbEntity.Relationship();
            result.Active = item.Active;
            result.Deleted = item.Deleted;
            result.AssignedTo = item.AssignedTo;
            result.PreviousNodeName = item.PreviousNodeName;
            result.NodeName = item.NodeName;
            result.RelationshipId = item.RelationshipId;
            result.Submitter = item.Submitter;
            result.TimeStamp = item.TimeStamp;
            result.TrackableName = item.TrackableName;
            result.TransactionName = item.TransactionName;
            result.Comment = item.Comment;
            int iType = (int)item.Type;
            result.Type = (pyprflow.Workflow.Model.TransactionType)iType;
           
            result.WorkflowName = item.WorkflowName;
            return result;

        }
        internal pyprflow.Workflow.Model.Relationship Map(pyprflow.DbEntity.Relationship item)
        {
            pyprflow.Workflow.Model.Relationship result =
                new pyprflow.Workflow.Model.Relationship();
            result.Active = item.Active;
            result.Deleted = item.Deleted;
            result.AssignedTo = item.AssignedTo;
            result.PreviousNodeName = item.PreviousNodeName;
            result.NodeName = item.NodeName;
            result.RelationshipId = item.RelationshipId;
            result.Submitter = item.Submitter;
            result.TimeStamp = item.TimeStamp;
            result.TrackableName = item.TrackableName;
            result.TransactionName = item.TransactionName;
            result.Comment = item.Comment;
            int iType = (int)item.Type;
            result.Type = (pyprflow.Workflow.Model.TransactionType)iType;

            result.WorkflowName = item.WorkflowName;
            return result;

        }

        internal Relationship CreateRelationshipObj(Transaction trans)
        {
            var r = new Relationship();
            r.TransactionName = trans.Name;
            r.TrackableName = trans.TrackableName;
            if (trans.type == TransactionType.move)
            {
                r.NodeName = trans.NewNodeId;
                r.PreviousNodeName = trans.CurrentNodeId;
            }
            else if (trans.type == TransactionType.copy)
            {
                r.PreviousNodeName = trans.CurrentNodeId;
                r.NodeName = trans.NewNodeId;
            }
            else if (trans.type == TransactionType.assignment)
                r.NodeName = trans.CurrentNodeId;
            else if (trans.type == TransactionType.comment)
            {
                r.Comment = trans.Comment;
                r.NodeName = trans.CurrentNodeId;
            }
            r.WorkflowName = trans.WorkflowName;
            if (trans.AssignedTo != null)
                r.AssignedTo = trans.AssignedTo.Email;
            r.Type = trans.type;
            if (trans.Submitter != null)
                r.Submitter = trans.Submitter.Email;
            Console.WriteLine("transacation type is {0}", trans.type);
            return r;
        }

        internal pyprflow.DbEntity.Relationship Create(Trackable trackable)
        {
            pyprflow.DbEntity.Relationship r = new pyprflow.DbEntity.Relationship(
                  pyprflow.Workflow.Model.TransactionType.add);
            r.TrackableName = trackable.Name;
            return r;

        }
        internal pyprflow.DbEntity.Relationship Create(Model.Workflow workflow)
        {
            pyprflow.DbEntity.Relationship r = new pyprflow.DbEntity.Relationship(
                pyprflow.Workflow.Model.TransactionType.add);
            r.WorkflowName = workflow.Name;
            return r;

        }

        internal List<pyprflow.Workflow.Model.Relationship> Map(List<pyprflow.DbEntity.Relationship> items)
        {
            List<pyprflow.Workflow.Model.Relationship> result = new List<Relationship>();
            foreach (pyprflow.DbEntity.Relationship item in items)
            {
                result.Add(Map(item));
            }
            return result;
        }

        
    }
}
