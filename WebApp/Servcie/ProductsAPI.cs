using AppUtility.APIRequest;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Newtonsoft.Json;
using Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Servcie
{
    public interface IProductsAPI
    {
        Task<IResponse<ProductDetails>> GetProductDetails(int Id);
        Task<IResponse<List<Filters>>> GetProductAttrDetails(int Id);
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
        public async Task<IResponse<List<Filters>>> GetProductAttrDetails(int Id)
        {
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/ProductHome/GetProductAttrDetails", JsonConvert.SerializeObject(new SearchItem { Id = Id }));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<ProductAttributes>>>(Response.Result);
                var distinctList = deserializeObject.Result.GroupBy(x => x.AttributeName).ToList();
                var filters = new Response<List<Filters>>
                {
                    ResponseText = deserializeObject.ResponseText,
                    StatusCode= deserializeObject.StatusCode,
                    Result = new List<Filters>()
                };
                foreach (var item in distinctList)
                {
                    var filter = new Filters { FilterName = item.Key};
                    var filterValues = new List<FilterValues>();
                    foreach (var val in item)
                    {
                        filter.FilterId = val.AttributeId;
                        filterValues.Add(new FilterValues { Value = val.AttributeValue});
                    }
                    filter.Values= filterValues;
                    filters.Result.Add(filter ?? new Filters());
                }
                return filters;
            }
            else
            {
                var res = new Response<List<Filters>>
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
