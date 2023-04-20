
using AppUtility.APIRequest;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentGateWay.PaymentGateway.PayU;
using Service.Models;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApp.Middleware;
using WebApp.Models;
namespace WebApp.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PGCallbackController : Controller
    {
        private string _apiBaseURL;
        public PGCallbackController(AppSettings appSettings)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }
        [Route("/PGCallback/PayUnotify")]
        public async Task<IActionResult> PayUnotify(PayUResponse request)
        {
            var res = new ResponsePG
            { 
            StatusCode=ResponseStatus.Failed,
            ResponseText="Failed"
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/PlaceOrder/PayUnotify", JsonConvert.SerializeObject(request), GetToken());
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
              res = JsonConvert.DeserializeObject<ResponsePG>(Response.Result);
            }
            StringBuilder html = new StringBuilder(@"<html><head><script>
                                (()=>{
                                        var obj={TID:""{TID}"",Amount:""{Amount}"",TransactionID:""{TransactionID}"",statuscode:""{status}"",reason:""{reason}"",origin:""addMoney"",gateway:""PayU""}
                                        localStorage.setItem('obj', JSON.stringify(obj));
                                        window.close()
                                   })();</script></head><body><h6>Redirect to site.....</h6></body></html>");
            html.Replace("{TID}", request.txnid);
            html.Replace("{Amount}", request.amount.ToString());
            html.Replace("{TransactionID}", request.mihpayid);
            html.Replace("{status}", res.StatusCode.ToString());
            html.Replace("{reason}", request.field9);
            return Content(html.ToString(), contentType: "text/html; charset=utf-8");
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
    }
}
