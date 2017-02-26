using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace workflow.Model
{
	public class WorkflowRepository : IWorkflowRepository
	{
		private static ConcurrentDictionary<string, Workflow> _Workflow =
			new ConcurrentDictionary<string, Workflow>();
		private static ConcurrentDictionary<string, Trackable> _Trackable =
				 new ConcurrentDictionary<string, Trackable>();
		public WorkflowRepository()
		{
			Add(new Workflow("_blank"));
			Trackable td = new Trackable("doc1") { TrackableId = "doc1" };
			td.Demo("_blank", "Step1");
			Trackable td2 = new Trackable("doc2") { TrackableId = "doc2" };
			td2.Demo("_blank", "Step1");
			Add(td);
			Add(td2);
		}
		public void Add(Workflow workflow)
		{
			if(workflow.Key == null || workflow.Key ==String.Empty )
				workflow.Key = Guid.NewGuid().ToString();
			_Workflow[workflow.Key] = workflow;
		}

		public void Add(Trackable trackable)
		{
			if (trackable.Key == null || trackable.Key == String.Empty)
				trackable.Key = Guid.NewGuid().ToString();
			_Trackable[trackable.Key] = trackable;
		}

		public bool CheckValidUserKey(string stringValue)
		{
			var userkeylist = new List<string>();
			userkeylist.Add("eric");
			userkeylist.Add("test");
			if(userkeylist.Contains(stringValue))
				return true;
			return false;
		}

		
		public Workflow Find(string key)
		{
			Workflow workflow;
			_Workflow.TryGetValue(key, out workflow);
			return workflow;
		}

		public Trackable FindTrackable(string key)
		{
			Trackable Trackable;
			_Trackable.TryGetValue(key, out Trackable);
			return Trackable;
		}

		public IEnumerable<Workflow> GetAll()
		{
			return _Workflow.Values;
		}

		public IEnumerable<Trackable> GetAllTrackable()
		{
			return _Trackable.Values;
		}

		public Workflow Remove(string key)
		{
			Workflow report; bool d;
			try { d =  _Workflow.TryRemove(key, out report); }
			catch (Exception ex)
			{
				throw new WorkFlowException("error TryRemove: "+ex.InnerException);
			}
			if (d)
				return report;
			return null;
		}

		public Trackable RemoveTrackable(string key)
		{
			Trackable report; bool d;
			try { d = _Trackable.TryRemove(key, out report); }
			catch (Exception ex)
			{
				throw new WorkFlowException("error TryRemove: " + ex.InnerException);
			}
			if (d)
				return report;
			return null;
		}

		public void Update(Workflow workflow)
		{
			_Workflow[workflow.Key] = workflow;
		}

		public void Update(Trackable Trackable)
		{
			_Trackable[Trackable.Key] = Trackable;
		}
	}
}
