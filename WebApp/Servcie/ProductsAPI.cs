using AppUtility.APIRequest;
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
    public interface IProductsAPI
    {
        Task<IResponse<ProductDetails>> GetProductDetails(int Id);
        Task<IResponse<List<ProductAttributes>>> GetProductAttrDetails(int Id);
        Task<IResponse<List<ProductPictureInfo>>> GetProductPicDetails(int Id);
    }
    public class ProductsAPI : IProductsAPI
    {
        private string _apiBaseURL;
        public ProductsAPI(AppSettings appSettings)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }
        public async Task<IResponse<ProductDetails>> GetProductDetails(int Id)
        {
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/ProductHome/GetProductDetails", JsonConvert.SerializeObject(new SearchItem { Id = Id}));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<ProductDetails>>(Response.Result);
                return deserializeObject;
            }
            else
            {
                var res = new Response<ProductDetails>
                {
                    StatusCode = ResponseStatus.Failed,
                    ResponseText = "Somthing Went Wrong",
                };
                return res;
            }
        }
        public async Task<IResponse<List<ProductAttributes>>> GetProductAttrDetails(int Id)
        {
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/ProductHome/GetProductAttrDetails", JsonConvert.SerializeObject(new SearchItem { Id = Id }));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<ProductAttributes>>>(Response.Result);
                return deserializeObject;
            }
            else
            {
                var res = new Response<List<ProductAttributes>>
                {
                    StatusCode = ResponseStatus.Failed,
                    ResponseText = "Somthing Went Wrong",
                };
                return res;
            }
        } 
        public async Task<IResponse<List<ProductPictureInfo>>> GetProductPicDetails(int Id)
        {
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/ProductHome/GetProductPicDetails", JsonConvert.SerializeObject(new SearchItem { Id = Id }));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<ProductPictureInfo>>>(Response.Result);
                return deserializeObject;
            }
            else
            {
                var res = new Response<List<ProductPictureInfo>>
                {
                    StatusCode = ResponseStatus.Failed,
                    ResponseText = "Somthing Went Wrong",
                };
                return res;
            }
        }
    }
}
