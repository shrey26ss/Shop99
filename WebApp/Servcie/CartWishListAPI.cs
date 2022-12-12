using AppUtility.APIRequest;
using Data.Models;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Servcie
{

    public interface ICartWishListAPI
    {
        Task<IResponse> AddWishList(WishList req);
        Task<IResponse<IEnumerable<CartItems>>> GetCartList();
    }
    public class CartWishListAPI : ICartWishListAPI
    {
        private string _apiBaseURL;
        public CartWishListAPI(AppSettings appSettings)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }
        public async Task<IResponse> AddWishList(WishList req)
        {
            var res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CartWishList/AddWishList", JsonConvert.SerializeObject(req));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    res = JsonConvert.DeserializeObject<Response>(Response.Result);
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;
           
        }

        public async Task<IResponse<IEnumerable<CartItems>>> GetCartList()
        {
            var res = new Response<IEnumerable<CartItems>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CartWishList/GetCartList", null);
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<CartItems>>>(Response.Result);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;
        }
    }
}
