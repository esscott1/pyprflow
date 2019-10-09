using System;
using System.Collections.Generic;
using System.Text;

namespace pyprflow.Workflow.Model.Reports
{
    public class ReportEngineContext
    {
        private readonly Dictionary<string, IReport> _strategies =
          new Dictionary<string, IReport>();
        protected IWorkflowRepository Repository { get; set; }
        public ReportEngineContext()
        {
            _strategies = new Dictionary<string, IReport>();
            _strategies.Add("agingreport", new AgingReport());
        }

      
        public IReport Run(ReportRequest request)
        {
            return  _strategies[request.ReportName].Run();
            
        }
      
    }
    public class ReportRequest
    {
        public ReportRequest(string reportType)
        {
            ReportName = reportType;
        }
        public string ReportName { get; set; }
    }

 
}
