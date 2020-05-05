using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleAuthPolicies.Authorization;
using SampleAuthPolicies.Models;

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

        [HttpGet("at-least-editor")]
        [Authorize(Policy = nameof(AuthorizationPolicies.AtLeastEditor))]
        public ActionResult<Response> GetAtLeastEditor()
            => new Response { Data = "Result from endpoint with [Authorize(Policy = nameof(AuthorizationPolicies.AtLeastEditor))]" };

        [HttpGet("admin-role-only")]
        [Authorize(Roles = "admin")]
        public ActionResult<Response> GetAdminOnly()
            => new Response { Data = "Result from endpoint with [Authorize(Roles = 'admin')]" };
    }
}
