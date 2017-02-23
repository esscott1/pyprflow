using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workflow.Model
{
	public interface IWorkflowRepository
	{
		void Add(Workflow workflow);
		IEnumerable<Workflow> GetAll();
		Workflow Find(string key);
		Workflow Remove(string key);
		void Update(Workflow workflow);
	}
}
