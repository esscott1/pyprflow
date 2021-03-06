﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pyprflow.DbEntity
{
    public abstract class BaseDbItem
    {
        public bool Active { get; set; } = true;
        public bool Deleted { get; set; } = false;
    }
    public class BaseDbWorkFlowItem : BaseDbItem
    {
        public BaseDbWorkFlowItem()
        {

        }

        public BaseDbWorkFlowItem(string serializedObject)
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

        public object Deserialize(string serializedObject, Type type)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            return JsonConvert.DeserializeObject(serializedObject, type, settings);
        }

        public T Deserialize<T>(string serializedObject)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            return JsonConvert.DeserializeObject<T>(serializedObject, settings);
        }
        public string Serialize(object item, Type type)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            return JsonConvert.SerializeObject(item, type, settings);
        }
        public string Serialize<T>(T item)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            return JsonConvert.SerializeObject(item, typeof(T), settings);
        }


        public string Name { get; set; }

        [JsonIgnore]

        public string DerivedType { get; set; }

        [JsonIgnore]
        public string SerializedObject { get; set; }
    }
}
