using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pyprflow.Workflow.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace pyprflow.Api.Controllers
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
            t.CurrentNodeId = null;
            t.Submitter = new User() { Email = "Sample.User@somewhere.com" };
            t.AssignedTo = new User() { Email = "worker@somewhere.com" };
            t.TrackableName = "SampleDoc1";
            t.WorkflowName = "SampleWorkflow1";
            t.type = TransactionType.move;
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
        [HttpGet("{id}", Name = "GetTransaction")]
        public IActionResult GetTransaction(string id)
        {
            return Json(Repository.Find<Transaction>(id));
        }

        // only allow Posts of transactions as they are not editable
        [HttpPost]
        public IActionResult SubmitTransaction([FromBody] Transaction trans)
        {
            //	http://www.newtonsoft.com/jsonschema/help/html/ValidatingJson.htm
            if (trans == null)
            { return BadRequest("trans is null"); }

            try
            {
                string msg = "error"; int statusCode = 400;
                if (trans.Execute(Repository, out statusCode, out msg))
                    return CreatedAtRoute("GetTransaction", new { id = trans.Name }, Repository);
                else
                    return StatusCode(statusCode, msg);

            }
            catch (Exception ex)
            { return (StatusCode(500, ex.Message)); }

        }

        [HttpPut("deactivate/{id}")]
        public IActionResult Deactivate(string id)
        {
            Repository.Deactivate<Transaction>(id);
            //    var _workflow = Repository.Find<Workflow>(id);
            return Json(String.Format("transaction with transactionID {0} is has been soft deleted", id));
        }

        [HttpDelete()]
        public void Delete([FromBody] Transaction trans)
        {
            Repository.EmptyAll();
        }


    }


}
