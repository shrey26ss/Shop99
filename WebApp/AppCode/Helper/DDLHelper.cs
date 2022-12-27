using AppUtility.APIRequest;
using AppUtility.Extensions;
using Entities.Models;
using Entities.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Service.Models;

namespace AppUtility.Helper
{
    public interface IDDLHelper
    {
        Task<List<CategoryDDL>> GetCategoryDDL(string _token, string _apiBaseURL);
        Task<List<AttributesDDL>> GetAttributeDDL(string _token, string _apiBaseURL);
        Task<List<AttributesDDL>> GetCategoryMappedAttributeDDL(string _token, string _apiBaseURL, int CatId);
        Task<List<BrandsDDL>> GetBrandsDDL(string _token, string _apiBaseURL);
        Task<List<StateDDL>> GetStateDDL(string _apiBaseURL);
    }
    public class DDLHelper : IDDLHelper
    {
        public async Task<List<CategoryDDL>> GetCategoryDDL(string _token, string _apiBaseURL)
        {
            var list = new List<CategoryDDL>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/GetList", null, _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var _ = JsonConvert.DeserializeObject<Response<IEnumerable<CategoryDDL>>>(apiResponse.Result);
                list = _.Result.ToList();
            }
            return list;
        }
        public async Task<List<AttributesDDL>> GetAttributeDDL(string _token, string _apiBaseURL)
        {
            var list = new List<AttributesDDL>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Attribute/GetAttributeDDL", null, _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var _ = JsonConvert.DeserializeObject<Response<IEnumerable<AttributesDDL>>>(apiResponse.Result);
                list = _.Result.ToList();
            }
            return list;
        }
        public async Task<List<AttributesDDL>> GetCategoryMappedAttributeDDL(string _token, string _apiBaseURL, int CatId)
        {
            var list = new List<AttributesDDL>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Attribute/GetCategoryMappedAttributeDDL", JsonConvert.SerializeObject(new SearchItem { Id = CatId }), _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var _ = JsonConvert.DeserializeObject<Response<IEnumerable<AttributesDDL>>>(apiResponse.Result);
                list = _.Result.ToList();
            }
            return list;
        }
        public async Task<List<BrandsDDL>> GetBrandsDDL(string _token, string _apiBaseURL)
        {
            var list = new List<BrandsDDL>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Brand/GetBrandDDL", null, _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var _ = JsonConvert.DeserializeObject<Response<IEnumerable<BrandsDDL>>>(apiResponse.Result);
                list = _.Result.ToList();
            }
            return list;
        }
        public async Task<List<StateDDL>> GetStateDDL(string _apiBaseURL)
        {
            var list = new List<StateDDL>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/State/GetStateDDL",null);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var _ = JsonConvert.DeserializeObject<IEnumerable<StateDDL>>(apiResponse.Result);
                list = _.ToList();
            }
            return list;
        }
    }
}
