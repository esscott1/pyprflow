using System;
using System.Collections.Generic;
using System.Text;

namespace pyprflow.Workflow.Model.SearchResult
{
    public class TrackableSearchResult : Trackable
    {
        public List<string> Locations { get; internal set; }
        public List<string> Comments { get; internal set; }
        public List<string> CurrentAssignment { get; internal set; }

        public TrackableSearchResult(Trackable trackable)
        {
            this.Name = trackable.Name;
            this.Locations = new List<string>();
            this.CurrentAssignment = new List<string>();
            this.Comments = new List<string>();

        }
    }
}
