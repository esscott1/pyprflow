﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pyprflow.Workflow.Model
{
	public class BaseWorkflowItem: BaseItem
	{
        public string Name { get; set; }
        public BaseWorkflowItem() {
          
        }

      
		public BaseWorkflowItem(string serializedObject)
		{
			SerializedObject = serializedObject;
		}

        //public BaseWorkflowItem GetBase<T>(T derivedObject) 
        //{
        //    BaseWorkflowItem result = new BaseWorkflowItem();
        //    result.Name = this.Name;
        //    result.Active = derivedObject.Active;
        //    result.SerializedObject = this.Serialize<T>(derivedObject);
        //    result.DerivedType = typeof(T).ToString();
        //    return result;
        //}
		
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

		
		
       
        [JsonIgnore]
	
		public string DerivedType { get; set; }
		
		[JsonIgnore]
		public string SerializedObject { get; set; }
	}
}
