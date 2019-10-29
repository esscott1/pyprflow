using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace pyprflow.Api.Controllers
{
    [Route("api/[controller]")]
    public class InstructionsController : ControllerBase
    {
       
        [HttpGet("instructions")]
        public ActionResult instructions()
        {

         //   var path = "./HTML/Instructions.html";
            var path = "/app/wwwroot/index.html";
            
            return new ContentResult()
            {
                Content = System.IO.File.ReadAllText(path),
                StatusCode = (int)HttpStatusCode.OK,
                ContentType = "text/html",
            };

        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "instruction1", "instruction2" };
        }


    }

   
}