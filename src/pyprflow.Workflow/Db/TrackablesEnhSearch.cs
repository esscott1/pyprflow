using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using pyprflow.Workflow.Model;
using pyprflow.Workflow.Model.SearchResult;

namespace pyprflow.Workflow.Db
{
    class TrackablesEnhSearch : TrackablesSearch, ISearchEngine
    {
        public TrackablesEnhSearch(IWorkflowRepository repository) : base(repository)
        { 
        }

        public override List<BaseWorkflowItem> Search(SearchRequest request)
        {
            List<BaseWorkflowItem> result = new List<BaseWorkflowItem>();


            if (request.Predicate == null)
            {
                var t = Repository.GetAll<Trackable>().Select(tr => tr.Name);
                foreach (string tn in t)
                {
                    var trackable = Repository.Find<Trackable>(tn);
                    // var trackableSearchResult = Augment(trackable, relationships);
                    var trackableSearchResult = Augment(trackable);
                    result.Add(trackableSearchResult);
                }
            }
            else
            {
                List<Relationship> relationships = Repository.Where(request.Predicate);
                var uniqueR2 = relationships.Select(x => x.TrackableName).Distinct().ToList();
                foreach (string tn in uniqueR2)
                {
                    var trackable = Repository.Find<Trackable>(tn);
                    // var trackableSearchResult = Augment(trackable, relationships);
                    var trackableSearchResult = Augment(trackable);
                    result.Add(trackableSearchResult);
                }
            }
            return result;
        }

        private TrackableSearchResult Augment(Trackable trackable)
        {
            TrackableSearchResult result = new TrackableSearchResult(trackable);
            //result.Locations = Locate(result.Name);
            var relationships = Repository.Where(r => r.TrackableName == result.Name && r.Active == true);// && (r.Type == Database.Entity.TransactionType.move || r.Type == Database.Entity.TransactionType.copy));
            foreach (Relationship rel in relationships)
            {
                if (rel.Type == TransactionType.copy || rel.Type == TransactionType.move)
                    result.Locations.Add(rel.NodeName);
                if (rel.Type == TransactionType.assignment)
                    result.CurrentAssignment.Add(rel.AssignedTo);
                if (rel.Type == TransactionType.comment)
                    result.Comments.Add(rel.Comment);
            }
            return result;
        }
    }
}
