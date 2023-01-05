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
    public interface ICategoryAPI
    {
        Task<IResponse<List<MenuItem>>> GetMenu();
        Task<IResponse<IEnumerable<Category>>> GetTopCategory();
        Task<IResponse<IEnumerable<TopBanner>>> GetTopBanner();
        Task<IResponse<IEnumerable<TopLowerBanner>>> GetTopLowerBanners();
        Task<IResponse<IEnumerable<ProductResponse>>> GetNewProducts(ProductRequest productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetBestSeller(ProductRequest productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetOnSaleProducts(ProductRequest productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetFeatureProducts(ProductRequest productRequest);
        Task<IResponse<IEnumerable<HotDealsResponse>>> GetHotDeals(ProductRequest productRequest);
        //Task<IResponse<IEnumerable<ProductResponse>>> ByCategoryProduct(ProductRequest productRequest);
        Task<IResponse<IEnumerable<Filters>>> GetCategoryFilters(ProductRequest productRequest);
        //Task<IResponse<IEnumerable<ProductResponse>>> ProductByProducId(ProductRequest productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetProducts(ProductRequest productRequest, string URL);
    }
    public class CategoryAPI : ICategoryAPI
    {
        private string _apiBaseURL;
        public CategoryAPI(AppSettings appSettings)
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

        public async Task<IResponse<IEnumerable<Category>>> GetTopCategory()
        {
            var res = new Response<IEnumerable<Category>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/TopCategory", null);
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<Category>>>(Response.Result);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;
        }


        //public async Task<IResponse<IEnumerable<ProductResponse>>> ByCategoryProduct(ProductRequest productRequest)
        //{
        //    var res = new Response<IEnumerable<ProductResponse>>
        //    {
        //        StatusCode = ResponseStatus.Failed,
        //        ResponseText = "Somthing Went Wrong",
        //    };
        //    var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Home/ByCategoryProduct", JsonConvert.SerializeObject(productRequest));
        //    if (Response.HttpStatusCode == HttpStatusCode.OK)
        //    {
        //        try
        //        {
        //            var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<ProductResponse>>>(Response.Result);
        //            return deserializeObject;
        //        }
        //        catch (Exception e)
        //        {
        //            res.ResponseText = e.Message;
        //        }
        //    }
        //    return res;
        //}

      


        public async Task<IResponse<IEnumerable<TopBanner>>> GetTopBanner()
        {
            var res = new Response<IEnumerable<TopBanner>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.CallUsingHttpWebRequest_GET($"{_apiBaseURL}/api/Home/TopBanners");
            if (!string.IsNullOrEmpty(Response))
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<TopBanner>>>(Response);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<TopLowerBanner>>> GetTopLowerBanners()
        {
            var res = new Response<IEnumerable<TopLowerBanner>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.CallUsingHttpWebRequest_GET($"{_apiBaseURL}/api/Home/TopLowerBanners");
            if (!string.IsNullOrEmpty(Response))
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<TopLowerBanner>>>(Response);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<ProductResponse>>> GetNewProducts(ProductRequest productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Home/NewArrivals",JsonConvert.SerializeObject(productRequest));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<ProductResponse>>>(Response.Result);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<ProductResponse>>> GetFeatureProducts(ProductRequest productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Home/FeatureProducts", JsonConvert.SerializeObject(productRequest));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<ProductResponse>>>(Response.Result);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<ProductResponse>>> GetBestSeller(ProductRequest productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Home/BestSellerProduct", JsonConvert.SerializeObject(productRequest));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<ProductResponse>>>(Response.Result);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<ProductResponse>>> GetOnSaleProducts(ProductRequest productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Home/OnSaleProducts", JsonConvert.SerializeObject(productRequest));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<ProductResponse>>>(Response.Result);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;
        }



        public async Task<IResponse<IEnumerable<HotDealsResponse>>> GetHotDeals(ProductRequest productRequest)
        {
            var res = new Response<IEnumerable<HotDealsResponse>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Home/HotDeals", JsonConvert.SerializeObject(productRequest));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<HotDealsResponse>>>(Response.Result);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<Filters>>> GetCategoryFilters(ProductRequest productRequest)
        {
            var res = new Response<IEnumerable<Filters>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Home/FiltersData", JsonConvert.SerializeObject(productRequest));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<Filters>>>(Response.Result);
                    return deserializeObject;
                }
                catch (Exception e)
                {
                    res.ResponseText = e.Message;
                }
            }
            return res;
        }
        //public async Task<IResponse<IEnumerable<ProductResponse>>> ProductByProducId(ProductRequest productRequest)
        //{
        //    var res = new Response<IEnumerable<ProductResponse>>
        //    {
        //        StatusCode = ResponseStatus.Failed,
        //        ResponseText = "Somthing Went Wrong",
        //    };
        //    var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Home/ByProductId", JsonConvert.SerializeObject(productRequest));
        //    if (Response.HttpStatusCode == HttpStatusCode.OK)
        //    {
        //        try
        //        {
        //            var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<ProductResponse>>>(Response.Result);
        //            return deserializeObject;
        //        }
        //        catch (Exception e)
        //        {
        //            res.ResponseText = e.Message;
        //        }
        //    }
        //    return res;
        //}
        public async Task<IResponse<IEnumerable<ProductResponse>>> GetProducts(ProductRequest productRequest, string URL)
        {
            var res = new Response<IEnumerable<ProductResponse>>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Somthing Went Wrong",
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}{URL}", JsonConvert.SerializeObject(productRequest));
            if (Response.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<IEnumerable<ProductResponse>>>(Response.Result);
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
