using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pyprflow.Workflow.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace pyprflow.Api.Controllers
{
    [Route("api/[controller]")]
    public class AdminController : BaseController
    {

        public AdminController(IWorkflowRepository workflow) :base(workflow)
        {
            Repository = workflow; 
        }

        //[HttpGet("version")]
        //public IActionResult Version()
        //{
        //    var ver = System.Reflection.Assembly.GetEntryAssembly().ImageRuntimeVersion;
        //    var msver = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion;
        //    return Json(msver);
        //}

        [HttpGet("db")]
        public IActionResult environment()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            var environment = String.Format("Running version {0} of Pyprflow", version);

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
