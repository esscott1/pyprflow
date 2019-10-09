using System;
using System.Collections.Generic;
using System.Text;

namespace pyprflow.Workflow.Model.SearchResult
{
    public class TrackableSearchResult : Trackable
    {
        public List<string> Locations { get; internal set; }
        public List<string> Comments { get; internal set; }
        public List<CurrentAssignment> CurrentAssignments { get; internal set; }

        public TrackableSearchResult(Trackable trackable)
        {
            this.Name = trackable.Name;
            this.Locations = new List<string>();
            this.CurrentAssignments = new List<CurrentAssignment>();
            this.Comments = new List<string>();

        }
      
    }
    public class CurrentAssignment
    {
        public CurrentAssignment(string assignedTo, DateTime assignedAt)
        {
            AssignedTo = assignedTo;
            AssignedAt = assignedAt;
        }
        public string AssignedTo { get; set; }
        public DateTime AssignedAt { get; set; }

    }
}
