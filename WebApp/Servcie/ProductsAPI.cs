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
        Task<IResponse<List<AttributeInfo>>> GetProductAttributeInfo(int Id);
        Task<IResponse<VariantIdByAttributesResponse>> GetVariantIdByAttributes(VariantIdByAttributesRequest request);
        Task<IResponse<ProductDetails>> GetProductAllDetails(int Id);
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
        public async Task<IResponse<ProductDetails>> GetProductAllDetails(int Id)
        {
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/ProductHome/GetProductAllDetails", JsonConvert.SerializeObject(new SearchItem { Id = Id }));
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
                    var filter = new Filters { Name = item.Key};
                    var filterValues = new List<FiltersAttributes>();
                    foreach (var val in item)
                    {
                        filter.AttributeId = val.AttributeId;
                        filterValues.Add(new FiltersAttributes { AttributeValue = val.AttributeValue, VariantId = val.VariantId});
                    }
                    filter.attributes = filterValues;
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
        public async Task<IResponse<List<AttributeInfo>>> GetProductAttributeInfo(int Id)
        {
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/ProductHome/GetProductAttributeInfo", JsonConvert.SerializeObject(new SearchItem { Id = Id }));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<AttributeInfo>>>(Response.Result);
                return deserializeObject;
            }
            else
            {
                var res = new Response<List<AttributeInfo>>
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

        public async Task<IResponse<VariantIdByAttributesResponse>> GetVariantIdByAttributes(VariantIdByAttributesRequest request)
        {
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/ProductHome/GetVariantIdByAttributes", JsonConvert.SerializeObject(request));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<VariantIdByAttributesResponse>>(Response.Result);
                return deserializeObject;
            }
            else
            {
                var res = new Response<VariantIdByAttributesResponse>
                {
                    StatusCode = ResponseStatus.Failed,
                    ResponseText = "Somthing Went Wrong",
                };
                return res;
            }
        }
    }
}
