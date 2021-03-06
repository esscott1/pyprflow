﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pyprflow.Workflow.Model
{
    public class Relationship : BaseItem
    {
		
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RelationshipId { get; set; }
		public string TransactionName { get; set; }
		public string  TrackableName { get; set; }
		public string PreviousNodeName { get; set; }
		public string  NodeName { get; set; }
		public string  WorkflowName { get; set; }
		public DateTime TimeStamp { get; set; }
		public TransactionType Type { get; set; }
		public string Submitter { get; set; }
		public string AssignedTo { get; set; }
        public string Comment { get; set; }
		public Relationship()
		{
			TimeStamp = DateTime.Now;
		}
        public Relationship(TransactionType type) : this()
        {
            Type = type;

        }
	}
}
