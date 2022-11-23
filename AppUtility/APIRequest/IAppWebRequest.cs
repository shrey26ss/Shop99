using System.Threading.Tasks;

namespace AppUtility.APIRequest
{
    public interface IAppWebRequest
    {
        Task<HttpResponse> PostAsync(string URL, string PostData, string ContentType, int timeout = 0);
    }
}
