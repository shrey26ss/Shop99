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
    public static class Websiteinfomation
    {
        public static int Id { get; set; }
        public static string Whitelogo { get; set; }
        public static string Coloredlogo { get; set; }
        public static string Companyname { get; set; }
        public static string Companydomain { get; set; }
        public static string CompanyemailID { get; set; }
        public static string Companymobile { get; set; }
        public static string Companyaddress { get; set; }
        public static string Footerdescription { get; set; }
        public static int LoginID { get; set; }
    }
    public class ChacheKeys
    {
        public const string WebsiteinfoModel = "WebsiteinfoModel";
    }
}
