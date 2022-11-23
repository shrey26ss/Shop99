using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AppUtility.APIRequest
{
    public class WebRequestModel
    {
        public string Response { get; set; }
        public string EncryptedData { get; set; }
    }

    public class HttpResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string HttpMessage{ get; set; }
        public string Result { get; set; }
    }
}
