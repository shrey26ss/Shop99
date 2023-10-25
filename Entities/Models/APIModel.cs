using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class APIModel
    {
        public int Id { get; set; }
        public string TID { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string EntryOn { get; set; }
        public string Method { get; set; }
        public string InitiationTime { get; set; }
        public bool IsIncomingOutgoing { get; set; }
    }
    public class APIModelResponse
    {
        public int statusCode { get; set; }
        public string responseText { get; set; }
        public object exception { get; set; }
        public List<APIModel> result { get; set; }
        public object keyVals { get; set; }
    }
}
