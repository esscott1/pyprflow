using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pyprflow.Model;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace pyprflow.Controllers
{
	[Route("api")]
	public class BaseController : Controller
    {
		public IWorkflowRepository Repository { get; set; }
		[HttpGet("version")]
		public IActionResult Index()
		{
			var ver = System.Reflection.Assembly.GetEntryAssembly().ImageRuntimeVersion;
			var msver = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion;
			return Json(msver);
		}

        [HttpGet("environment")]
        public IActionResult environment()
        {
            var environment = Environment.GetEnvironmentVariable("database");
           
            return Json(environment);

        }
	}
}
