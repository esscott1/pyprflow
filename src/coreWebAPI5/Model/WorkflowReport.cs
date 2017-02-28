﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workflow.Model
{
    internal class NodeReport : Node
    {
		public List<Trackable> trackables { get; set; }

		public NodeReport (string name) : base(name)
		{

		}
		
    }
}
