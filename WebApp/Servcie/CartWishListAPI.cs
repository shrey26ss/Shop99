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
        Task<IResponse> AddWishList(WishList req, string _token);
        Task<IResponse> AddToCart(CartItem req, string _token);
        Task<IResponse<IEnumerable<WishListSlide>>> GetWishListSlide(string _token);
        Task<IResponse<IEnumerable<CartItemSlide>>> GetCartListSlide(string _token, bool IsBuyNow = false);
        Task<IResponse> DeleteCart(CartItem req, string _token);
        Task<IResponse<IEnumerable<CartWishlistCount>>> GetCartwishListCount(string _token);
        Task<IResponse> MoveItemWishListToCart(int Id, string _token);
        Task<IResponse> DeleteWishListItem(int Id, string _token);
        Task<IResponse> MoveAllItemWishListToCart(string _token);
    }
    public class CartWishListAPI : ICartWishListAPI
    {
        private string _apiBaseURL;
        public CartWishListAPI(AppSettings appSettings)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }
        public async Task<IResponse> AddWishList(WishList req, string _token)
        {
            var res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };

            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CartWishList/AddWishList", JsonConvert.SerializeObject(req), _token);
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
        public async Task<IResponse> AddToCart(CartItem req, string _token)
        {
            var res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CartWishList/AddCartItem", JsonConvert.SerializeObject(req), _token);
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


        public async Task<IResponse> DeleteCart(CartItem req, string _token)
        {
            var res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CartWishList/DeleteCart", JsonConvert.SerializeObject(req), _token);
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
        public async Task<IResponse<IEnumerable<WishListSlide>>> GetWishListSlide(string _token)
        {
            var res = new Response<IEnumerable<WishListSlide>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };

            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CartWishList/GetWishlist", null, _token);
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<WishListSlide>>>(Response.Result);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;

        }
        public async Task<IResponse<IEnumerable<CartItemSlide>>> GetCartListSlide(string _token,bool IsBuyNow = false)
        {
            var res = new Response<IEnumerable<CartItemSlide>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            string url = $"{_apiBaseURL}/api/CartWishList/GetCartItemlist?IsBuyNow={IsBuyNow}";
            var Response = await AppWebRequest.O.PostAsync(url.ToLower(), null, _token);
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<CartItemSlide>>>(Response.Result);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<CartWishlistCount>>> GetCartwishListCount(string _token)
        {
            var res = new Response<IEnumerable<CartWishlistCount>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };

            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CartWishList/CartWishListCount", null, _token);
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<CartWishlistCount>>>(Response.Result);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;

        }

        public async Task<IResponse> MoveItemWishListToCart(int Id, string _token)
        {
            var res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };

            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CartWishList/AddWishListToCart", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
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
        public async Task<IResponse> DeleteWishListItem(int Id, string _token)
        {
            var res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };

            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CartWishList/DeleteWishListItem", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
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
        public async Task<IResponse> MoveAllItemWishListToCart(string _token)
        {
            var res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };

            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CartWishList/MoveAllItemWishListToCart", null, _token);
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
    }
}
