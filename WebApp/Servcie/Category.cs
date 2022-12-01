using AppUtility.APIRequest;
using Data.Models;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Newtonsoft.Json;
using Service.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Servcie
{
    public interface ICategory
    {
        Task<IResponse<List<MenuItem>>> GetMenu();
    }
    public class Category : ICategory
    {
        private string _apiBaseURL;
        public Category(AppSettings appSettings)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }
        public async Task<IResponse<List<MenuItem>>> GetMenu()
        {
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/GetMenu", JsonConvert.SerializeObject(new Request()));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<MenuItem>>>(Response.Result);
                return deserializeObject;
            }
            else
            {
                var res = new Response<List<MenuItem>>
                {
                    StatusCode = ResponseStatus.Failed,
                    ResponseText = "Somthing Went Wrong",
                };
                return res;
            }

        }

        //public async Task<IResponse<List<MenuItem>>> GetTopCategory()
        //{
        //    var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/TopCategory",null);
        //    if (Response.HttpStatusCode == HttpStatusCode.OK)
        //    {
        //        var deserializeObject = JsonConvert.DeserializeObject<Response<List<MenuItem>>>(Response.Result);
        //        return deserializeObject;
        //    }
        //    else
        //    {
        //        var res = new Response<List<MenuItem>>
        //        {
        //            StatusCode = ResponseStatus.Failed,
        //            ResponseText = "Somthing Went Wrong",
        //        };
        //        return res;
        //    }

        //}
    }
}
