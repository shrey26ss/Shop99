using System.Threading.Tasks;

namespace AppUtility.APIRequest
{
    public interface IAppWebRequest
    {
        Task<HttpResponse> PostAsync(string URL, string PostData, string AccessToken = "", string ContentType = "application/json", int timeout = 0);
        Task<string> CallUsingHttpWebRequest_GET(string URL);
    }
}
