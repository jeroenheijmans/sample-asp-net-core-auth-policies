using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleAuthPolicies.Controllers
{
    [ApiController]
    [Route("endpoint-specific-security")]
    public class EndpointSpecificAuthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Response> Get() 
            => new Response { Data = "Result from endpoint without Authorize attribute" };

        [HttpGet("allow-anonymous")]
        [AllowAnonymous]
        public ActionResult<Response> GetAllowAnonymous()
            => new Response { Data = "Result from endpoint with [AllowAnonymous]" };

        [HttpGet("plain-authorize")]
        [Authorize]
        public ActionResult<Response> GetPlainAuthorize()
            => new Response { Data = "Result from endpoint with plainly [Authorize]" };

        [HttpGet("admin-role-only")]
        [Authorize(Roles = "admin")]
        public ActionResult<Response> GetAdminOnly()
            => new Response { Data = "Result from endpoint with [Authorize(Roles = \"admin\")]" };
    }

    public class Response
    {
        public DateTime Timestamp => DateTime.Now;
        public string Data { get; set; } = "Default data";
    }
}
