using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace pyprflow.Model
{
 //   public class TrackableRepository  
 //   {
	//	private static ConcurrentDictionary<string, Trackable> _Trackable =
	//			 new ConcurrentDictionary<string, Trackable>();
	//	public TrackableRepository()
	//	{
	//		Add(new Trackable("defaultdoc"));
	//	}
	//	public void Add(Trackable trackable)
	//	{
	//		if (trackable.Name == null || trackable.Name == String.Empty)
	//			trackable.Name = Guid.NewGuid().ToString();
	//		_Trackable[trackable.Name] = trackable;
	//	}

	//	public Trackable Find(string key)
	//	{
	//		Trackable Trackable;
	//		_Trackable.TryGetValue(key, out Trackable);
	//		return Trackable;
	//	}

	//	public IEnumerable<Trackable> GetAll()
	//	{
	//		return _Trackable.Values;
	//	}

	//	public Trackable Remove(string key)
	//	{
	//		Trackable report; bool d;
	//		try { d = _Trackable.TryRemove(key, out report); }
	//		catch (Exception ex)
	//		{
	//			throw new WorkFlowException("error TryRemove: " + ex.InnerException);
	//		}
	//		if (d)
	//			return report;
	//		return null;
	//	}

	//	public void Update(Trackable Trackable)
	//	{
	//		_Trackable[Trackable.Name] = Trackable;
	//	}
	//}
}

