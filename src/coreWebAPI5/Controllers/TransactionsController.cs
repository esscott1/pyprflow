﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using workflow.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace workflow.Controllers
{

	[Route("api/[controller]")]
	public class TransactionsController : BaseController
	{
		public TransactionsController(IWorkflowRepository workflow)
		{
			Repository = workflow;
		}
		[HttpGet("example")]
		public IActionResult GetSample()
		{
			Transaction t = new Transaction();
			t.Comment = "Submitting to workflow";
			//t.Name = "SampleTransaction1";
			t.NewNodeId = "SampleNode1";
			t.PreviousNodeId = null;
			t.Submitter = new User() { Email = "Sample.User@somewhere.com" };
			t.TrackableName = "SampleDoc1";
			t.WorkflowName = "SampleWorkflow1";
			t.type = TransactionType.Move;
			var d = DateTime.Now;
			t.TransActionTime = DateTime.Now;
			t.Name = "SampleTransaction1";
			return Json(t);

		}
		[HttpGet]
		public IActionResult GetAll()
		{
			//return Json("stuff");
			return Json(Repository.GetAll<Transaction>());
		}
	//	GET: api/values
	   [HttpGet("{id}",Name = "GetTransaction")]
		public IActionResult GetTransaction(string id)
		{
			return Json(Repository.Find<Transaction>(id));
		}

		// only allow Posts of transactions as they are not editable
		[HttpPost]
		public IActionResult SubmitTransaction([FromBody] Transaction trans)
		{
			
			if (trans == null)
			{ return BadRequest("trans is null"); }
			try
			{
				string msg; int statusCode;
				if (!trans.IsValid(Repository, out statusCode, out msg))
					return StatusCode(statusCode, msg);
			
				Console.WriteLine("passed IsValid validation");
				Repository.Add(trans);
				Repository.Track(trans);
				return CreatedAtRoute("GetTransaction", new { id = trans.Name }, Repository);
				
			}
			catch(Exception ex)
			{ return (StatusCode(500, ex.InnerException)); }

		}

		
		}

	
}
