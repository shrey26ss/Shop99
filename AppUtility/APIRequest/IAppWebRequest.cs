using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppUtility.APIRequest
{
    public interface IAppWebRequest
    {
        Task<HttpResponse> PostAsync(string URL, string PostData = "", string AccessToken = "", string ContentType = "application/json", int timeout = 0);
        Task<string> CallUsingHttpWebRequest_GET(string URL);
        Task<string> PostJsonDataUsingHWRTLS(string URL, object PostData, IDictionary<string, string> headers);
        Task<string> CallUsingHttpWebRequest_POSTAsync(string URL, string PostData, string ContentType = "application/x-www-form-urlencoded");
        Task<string> PostJsonDataUsingHWRTLS(string URL, object PostData);
        string CallUsingHttpWebRequest_POST(string URL, string PostData, string ContentType = "application/x-www-form-urlencoded");
    }
}
