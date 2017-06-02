using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace pyprflow.Model
{
	public interface ITrackableRepository
	{
		void Add(Trackable workflow);
		IEnumerable<Trackable> GetAll();
		Trackable Find(string key);
		Trackable Remove(string key);
		void Update(Trackable workflow);
		bool CheckValidUserKey(string stringValues);
	}
}