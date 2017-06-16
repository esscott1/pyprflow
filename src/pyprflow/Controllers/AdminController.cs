using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pyprflow.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace pyprflow.Controllers
{
    [Route("api/[controller]")]
    public class AdminController : BaseController
    {

        public AdminController(IWorkflowRepository workflow)
        {
            Repository = workflow;
        }

        [HttpGet("version")]
        public IActionResult Version()
        {
            var ver = System.Reflection.Assembly.GetEntryAssembly().ImageRuntimeVersion;
            var msver = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion;
            return Json(msver);
        }

        [HttpGet("db")]
        public IActionResult environment()
        {
            var environment = Environment.GetEnvironmentVariable("pyprflowDbType");

            return Json(environment);

        }
        // DELETE api/values/5
        [HttpDelete("all")]
        public void Delete()
        {
            Repository.EmptyAll();
        }
    }
}
