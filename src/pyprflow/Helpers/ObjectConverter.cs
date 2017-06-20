using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pyprflow.Model;

namespace pyprflow.Helpers
{
    public class ObjectConverter
    {
        public BaseWorkflowItem GetBase<T>(T derivedObject) where T : BaseWorkflowItem
        {
            BaseWorkflowItem result = new BaseWorkflowItem();
            result.Name = derivedObject.Name;
            result.Active = derivedObject.Active;
            result.SerializedObject = derivedObject.Serialize<T>(derivedObject);
            result.DerivedType = typeof(T).ToString();
            return result;
        }
    }
}
