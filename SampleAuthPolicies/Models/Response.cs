using System;

namespace SampleAuthPolicies.Models
{
    public class Response
    {
        public DateTime Timestamp => DateTime.Now;
        public string Data { get; set; } = "Default data";
    }
}
