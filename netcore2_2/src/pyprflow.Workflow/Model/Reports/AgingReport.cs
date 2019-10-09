using System;
using System.Collections.Generic;
using System.Text;

namespace pyprflow.Workflow.Model.Reports
{
    public class AgingReport : IReport
    {
        public AgingReport()
        {

        }
        public string TrackableName { get; set; }
        public TimeSpan IdileDuration { get; set; }
        public TimeSpan AssignedDuration { get; set; }
        public TimeSpan NodeDuration { get; set; }

        public IReport Run() {
            return new AgingReport()
            {
                TrackableName = "sampletrackable",
                IdileDuration = new TimeSpan(4, 30, 30),
                AssignedDuration = new TimeSpan(5, 45, 45),
                NodeDuration = new TimeSpan(6, 15, 30)
            };
        }
    }
}
