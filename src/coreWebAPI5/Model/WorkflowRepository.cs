﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreWebAPI5.Model
{
	public class WorkflowRepository : IWorkflowRepository
	{
		private static ConcurrentDictionary<string, Workflow> _Workflow =
			new ConcurrentDictionary<string, Workflow>();
		public WorkflowRepository()
		{
			Add(new Workflow("_blank"));
		}
		public void Add(Workflow workflow)
		{
			if(workflow.Key == null || workflow.Key ==String.Empty )
				workflow.Key = Guid.NewGuid().ToString();
			_Workflow[workflow.Key] = workflow;
		}

		public Workflow Find(string key)
		{
			Workflow workflow;
			_Workflow.TryGetValue(key, out workflow);
			return workflow;
		}

		public IEnumerable<Workflow> GetAll()
		{
			return _Workflow.Values;
		}

		public Workflow Remove(string key)
		{
			Workflow report;
			_Workflow.TryRemove(key, out report);
			return report;
		}

		public void Update(Workflow workflow)
		{
			_Workflow[workflow.Key] = workflow;
		}
	}
}