using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class NewsLetter
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CeratedOn { get; set; }
    }
    public class NewsLetterResponse
    {
        public int statusCode { get; set; }
        public string responseText { get; set; }
        public object exception { get; set; }
        public List<NewsLetter> result { get; set; }
        public object keyVals { get; set; }
    }
}
