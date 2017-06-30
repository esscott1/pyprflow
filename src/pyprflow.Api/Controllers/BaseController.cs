using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pyprflow.Workflow.Model;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace pyprflow.Api.Controllers
{
	[Route("api")]
	public class BaseController : Controller
    {
		public IWorkflowRepository Repository { get; set; }
       
        public BaseController(IWorkflowRepository repository)
        {
            Repository = repository;
        }

		[HttpGet("version")]
		public IActionResult Index()
		{
			var ver = System.Reflection.Assembly.GetEntryAssembly().ImageRuntimeVersion;
			var msver = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion;
			return Json(msver);
		}
        //[HttpPut("deactivate/{id}")]
        //public abstract IActionResult Deactivate(string id);
        ////{
        //    //Repository.Deactivate<BaseWorkflowItem>(id);
        //    ////    var _workflow = Repository.Find<Workflow>(id);
        //    //return Json(String.Format("transaction with transactionID {0} is has been soft deleted", id));
        //}

        internal IActionResult Deactivate<T>(string id) where T : BaseWorkflowItem
        {
            Repository.SoftDelete<T>(id);
            return Json(String.Format("the item with Id {0} was soft deleted", id));

        }






    }
}
