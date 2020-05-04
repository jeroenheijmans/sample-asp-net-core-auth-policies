using System;

namespace SampleAuthPolicies.Controllers
{
    public class Response
    {
        public DateTime Timestamp => DateTime.Now;
        public string Data { get; set; } = "Default data";
    }
}
