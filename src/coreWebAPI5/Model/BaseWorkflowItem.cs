using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace workflow.Model
{
	public class BaseWorkflowItem
	{
		public BaseWorkflowItem() { }
		public BaseWorkflowItem(string serializedObject)
		{
			SerializedObject = serializedObject;
		}

		
		public T Deserialize<T>(string serializedObject)
		{
			var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
			return JsonConvert.DeserializeObject<T>(serializedObject, settings);
		}

		public string Serialize<T>(T item)
		{
			var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
			return JsonConvert.SerializeObject(item, typeof(T), settings);
		}

		//[Name]
		//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		//[Key, Column(Order = 0)]
		public string Name { get; set; }
		[JsonIgnore]
		//[Key, Column(Order =1)]
		public string DerivedType { get; set; }
		//[Name]
		//public string Name { get; set;}
		[JsonIgnore]
		public string SerializedObject { get; set; }
	}
}
