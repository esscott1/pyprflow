using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using pyprflow.Api.Entities;
using pyprflow.Api.Services;

namespace pyprflow.Api.Controllers
{
  [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ValuesController : Controller
    {

        private readonly DatabaseSettings _databaseSettings;
        private IUserService _userService;
        public ValuesController(IOptions<DatabaseSettings> databaseSettings, IUserService userService)
        {
            _databaseSettings = databaseSettings.Value;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]User user)
        {
            IActionResult response = Unauthorized();
            User authUser = _userService.Authenticate(user.Username, user.Password);
            if(authUser != null)
            {
                response = Ok(new { token = authUser.Token });
            }
            return response;

        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2", System.IO.Directory.GetCurrentDirectory() };
        }

        [HttpGet("dir")]
        public IEnumerable<string> GetDir()
        {
            return System.IO.Directory.EnumerateDirectories(System.IO.Directory.GetCurrentDirectory());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
   //     [HttpPost]
   //     public string Post([FromBody]string value)
   //     {
			//return "i sent " + value;
   //     }

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
