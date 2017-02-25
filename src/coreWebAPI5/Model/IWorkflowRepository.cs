using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace workflow.Model
{
	public interface IWorkflowRepository
	{
		void Add(Workflow workflow);
		IEnumerable<Workflow> GetAll();
		Workflow Find(string key);
		Workflow Remove(string key);
		void Update(Workflow workflow);
		void Add(Trackable workflow);
		IEnumerable<Trackable> GetAllTrackable();
		Trackable FindTrackable(string key);
		Trackable RemoveTrackable(string key);
		void Update(Trackable workflow);
		bool CheckValidUserKey(string stringValues);
	}
}
