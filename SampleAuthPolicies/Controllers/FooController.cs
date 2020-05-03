using System;
using Microsoft.AspNetCore.Mvc;

namespace SampleAuthPolicies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FooController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Response> Get()
        {
            return new Response { Data = "Unprotected endpoint." };
        }
    }

    public class Response
    {
        public DateTime Timestamp => DateTime.Now;
        public string Data { get; set; } = "Default data";
    }
}
