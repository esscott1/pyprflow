using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyprflow.Workflow.Model
{
    public abstract class BaseItem
    {
        public bool Active { get; set; } = true;
        public bool Deleted { get; set; } = false;
    }
}
