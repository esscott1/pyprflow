using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace workflow.Model
{
	public interface IWorkflowRepository
	{

		IEnumerable<T> GetAll<T>();
		void Add(Workflow workflow);
		IEnumerable<Workflow> GetAll();
		Workflow Find(string key);
		Workflow Remove(string key);
		void Update(Workflow workflow);

		void Add(Trackable trackable);
		IEnumerable<Trackable> GetAllTrackable();
		Trackable FindTrackable(string key);
		Trackable RemoveTrackable(string key);
		void Update(Trackable workflow);
		
		void Add(ExecutedMove executedMove);
		IEnumerable<ExecutedMove> GetAllExecutedMoves();
		ExecutedMove FindExecutedMove(string key);
		IEnumerable<ExecutedMove> FindExecutedMoves(string trackableId);
		
		void Add(Transaction trans);
		IEnumerable<Transaction> GetAllTransactions();
		Transaction FindTransaction(string id);


		bool CheckValidUserKey(string stringValues);
	}
}
