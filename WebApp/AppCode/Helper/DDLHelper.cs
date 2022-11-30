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
    public class DDLHelper
    {
        public static DDLHelper O => instance.Value;
        private static Lazy<DDLHelper> instance = new Lazy<DDLHelper>(() => new DDLHelper());
        private DDLHelper() { }
        public string GetErrorDescription(int errorCode)
        {
            string error = ((Errorcodes)errorCode).DescriptionAttribute();
            return error;
        }
        public async Task<List<CategoryDDL>> GetCategoryDDL(string _token, string _apiBaseURL)
        {
            var list = new List<CategoryDDL>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/GetCategoriesDDL", null, _token);
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
    }
}
