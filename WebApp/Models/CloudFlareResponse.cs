using System.Collections.Generic;

namespace WebApp.Models
{
    public class CloudFlareResponse
    {
        public Result result { get; set; }
        public bool success { get; set; }
        public List<Error> errors { get; set; }
    }
    public class Result
    {
        public List<string> variants { get; set; }
    }
    public class Error
    {
        public int code { get; set; }
        public string message { get; set; }
    }
}
