using System;
using System.Collections.Generic;
using System.Text;

namespace pyprflow.Workflow.Model
{
    internal class TransactionValidator
    {
        private IWorkflowRepository _repository;
        private Transaction _trans;
        private int _statuscode = 0;
        private string _statusmessage = "success";

        internal TransactionValidator(IWorkflowRepository repository)
        {
            _repository = repository;
        }

     
        internal bool IsValid(Transaction trans, out int statusCode, out string statusMessage)
        {
            _trans = trans;
            bool result = true;
            statusCode = _statuscode;statusMessage = _statusmessage;
         
                // should be doing all Async  https://stackoverflow.com/questions/12337671/using-async-await-for-multiple-tasks
            result = DoesTrackableExist();
                if (!result) { 
                    statusCode = _statuscode; statusMessage = _statusmessage; return result; }

            result = DoesWorkflowExist();
                if (!result) { 
                    statusCode = _statuscode; statusMessage = _statusmessage; return result; }

            result = IsUniqueTransactionId();
                if (!result) {
                    statusCode = _statuscode; statusMessage = _statusmessage; return result; }

            result = IsTrackableInAcceptableNode();
                if (!result) { 
                         statusCode = _statuscode; statusMessage = _statusmessage; return result; }
            switch (_trans.type)
                {
                    case TransactionType.assignment:
                        result = IsValidAssignmentTransaction();
                        break;
                    case TransactionType.comment:
                        result = IsValidCommentTransaction();
                        break;
                    default: // move or copy
                        break;
                }
            { statusCode = _statuscode; statusMessage = _statusmessage; return result; }
        }

        private bool DoesWorkflowExist()
        {
            bool result = true;
            var workflow = _repository.Exist<Workflow>(_trans.WorkflowName);
            if (workflow == null)
            {
                _statuscode = 400;
                _statusmessage = string.Format("The workflowId {0} does not exists", _trans.WorkflowName);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Determines if the Trackable exists in the system.
        /// </summary>
        /// <returns></returns>
        private bool DoesTrackableExist()
        {
            bool result = true;
            if(!_repository.Exist<Trackable>(_trans.TrackableName))
            {
                _statuscode = 400;
                _statusmessage = string.Format("The trackable {0} does not exists", _trans.TrackableName);
                result = false;
            }
            return result;

        }

        /// <summary>
        /// Determine if the trackable is in the node the transaction says it is.
        /// </summary>
        /// <returns></returns>
        private bool IsTrackableInAcceptableNode()
        {
            // if below is true it's a new submission and it's up to the workflow object to determine 
            // acceptance into the workflow.
            if (_trans.CurrentNodeId == null && _trans.NewNodeId != null) 
                return true;
            bool result = true;
            Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dic =
                new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
            dic.Add("isactive", "true");
            dic.Add("nodeid", _trans.CurrentNodeId);
            dic.Add("trackableid", _trans.TrackableName);
            dic.Add("entityType", "trackables");
            Console.WriteLine("looking for trackables in {0} with trackableId = {1}", _trans.CurrentNodeId, _trans.TrackableName);
            Db.SearchRequest request = new Db.SearchRequest(dic);
            Db.SearchEngine se = new Db.SearchEngine(_repository);

            var response = se.Search(request);
            //Console.WriteLine("found {0} number of {1} in node {2}", response.Count, this.TrackableName, this.CurrentNodeId);
            if (response.Count == 0)
            {
                _statuscode = 400;
                _statusmessage = string.Format("Trackable {0} is not in the previous node {1} like you say it is", _trans.TrackableName, _trans.CurrentNodeId);
                result = false;
            }

            return result;

        }

        /// <summary>
        /// Determines if the Transaction has a unique Transaction Id.
        /// </summary>
        /// <returns></returns>
        private bool IsUniqueTransactionId()
        {
            bool result = true;
            if (_repository.Exist<Transaction>(_trans.Name))
            {
                _statuscode = 400;
               _statusmessage = string.Format("The transactionId {0} already exists", _trans.Name);
                result = false;
            }
            return result;

        }

        /// <summary>
        /// Determines if the transaction has sufficent information to be an assignment transaction
        /// </summary>
        /// <returns></returns>
        private bool IsValidAssignmentTransaction()
        {
            bool result = (!string.IsNullOrEmpty(_trans.CurrentNodeId) &&
                !string.IsNullOrEmpty(_trans.Submitter.Email) &&
                !string.IsNullOrEmpty(_trans.TrackableName) &&
                !string.IsNullOrEmpty(_trans.WorkflowName));
            if (!result)
            {
                _statuscode = 400;
                _statusmessage = "missing a required value";
            }
            return result;
        }

        /// <summary>
        /// Determines if the transaction has sufficent information to be a comment transaction
        /// </summary>
        /// <returns></returns>
        private bool IsValidCommentTransaction()
        {
            bool result = (!string.IsNullOrEmpty(_trans.CurrentNodeId) &&
                !string.IsNullOrEmpty(_trans.Submitter.Email) &&
                !string.IsNullOrEmpty(_trans.TrackableName) &&
                !string.IsNullOrEmpty(_trans.WorkflowName) &&
                !string.IsNullOrEmpty(_trans.Comment));
            if (!result)
            {
                _statuscode = 400;
                _statusmessage = string.Format("missing a required value, you sent {0}, {1},{2}, {3}, {4}",
                    _trans.CurrentNodeId, _trans.Submitter.Email, _trans.TrackableName, _trans.WorkflowName, _trans.Comment);
            }

            return result;

        }
        
        private bool IsValidMoveOrCopy()
        {
            bool result=true;
           
            return result;
        }

        private bool IsMoveValid(out int statusCode, out string statusMessage)
        {
            statusCode = 0;
            statusMessage = "success";
            return true;
        }
    }
}
