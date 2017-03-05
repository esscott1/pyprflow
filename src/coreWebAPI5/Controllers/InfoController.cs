using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using workflow.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace workflow.Controllers
{
	[Route("api/[controller]/[action]")]
	public class InfoController : Controller
	{
		public IWorkflowRepository Repo { get; set; }
		public InfoController(IWorkflowRepository workflow)
		{
			Repo = workflow;
		}
		// GET: api/values
		[HttpGet("{id}")]
        public IActionResult Workflow(string Id)
        {
			return Json(Repo.Find<Workflow>(Id));
        }

		[HttpGet("{id}")]
		public IActionResult Node(string Id)
		{
			return BadRequest();
		}
		[HttpGet("{id}")]
		public IActionResult Trackable(string Id)
		{
			return Json(Repo.Find<Trackable>(Id));
		}


		// GET api/values/5
		[HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
