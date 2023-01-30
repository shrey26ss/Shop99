using AppUtility.APIRequest;
using AppUtility.Helper;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApp.AppCode;
using WebApp.AppCode.Attributes;
using WebApp.AppCode.Helper;
using WebApp.Middleware;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        private string _apiBaseURL;
        private readonly Dictionary<string, string> _ImageSize;
        private readonly IHttpRequestInfo _httpInfo;
        private readonly IDDLHelper _ddl;
        private readonly ILogger<ProductController> _logger;
        public ProductController(AppSettings appSettings, IOptions<ImageSize> imageSize, IHttpRequestInfo httpInfo, IDDLHelper ddl, ILogger<ProductController> logger)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _ImageSize = imageSize.Value;
            _httpInfo = httpInfo;
            _ddl = ddl;
            _logger = logger;
        }

        #region Add Product
        // GET: ProductController
        [HttpGet("/Product")]
        public IActionResult Index()
        {
            var model = new ProductViewModel();
            model.Categories = _ddl.GetCategoryDDL(GetToken(), _apiBaseURL).Result;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProductList(int CID, string SearchText)
        {
            var response = new List<Products>();
            string _token = User.GetLoggedInUserToken();
            var jsonData = JsonConvert.SerializeObject(new ProductSearchItem { CategoryID = CID, SearchText = SearchText });
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/GetProducts", jsonData, _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<Products>>>(apiResponse.Result);
                response = deserializeObject.Result;
            }
            return PartialView("Partials/_ProductList", response);
        }
        [HttpGet]
        public async Task<IActionResult> Attributes(int Id, StatusType s)
        {
            var response = new AttrinutesViewModel();
            response.Id = Id;
            response.statusid = s;
            //var response = new List<ProductVariantAttributeDetails>();
            //string _token = User.GetLoggedInUserToken();
            //var jsonData = JsonConvert.SerializeObject(new SearchItem { Id = Id, StatusID = s });
            //var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/GetProductVarAttrDetails", jsonData, _token);
            //if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            //{
            //    var deserializeObject = JsonConvert.DeserializeObject<Response<List<ProductVariantAttributeDetails>>>(apiResponse.Result);
            //    response = deserializeObject.Result;
            //}
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetAttributes(int Id, StatusType s)
        {
            var response = new List<ProductVariantAttributeDetails>();
            string _token = User.GetLoggedInUserToken();
            var jsonData = JsonConvert.SerializeObject(new SearchItem { Id = Id, StatusID = s });
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/GetProductVarAttrDetails", jsonData, _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<ProductVariantAttributeDetails>>>(apiResponse.Result);
                response = deserializeObject.Result;
            }
            return PartialView("Partials/GetAttributes", response);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAdminApprovelStatus(int Id, StatusType StatusID,string Remark = "")
        {
            var response = new Response();
            string _token = User.GetLoggedInUserToken();
            var jsonData = JsonConvert.SerializeObject(new UpdateAdminApprovelStatus { Id = Id,Remark = Remark,StatusID = StatusID });
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/UpdateAdminApprovelStatus", jsonData, _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                response = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
            }
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateIsPublishVariant(UpdateIsPublishProduct req)
        {
            var res = new Response();
            if (req.ID >= 1)
            {
                string _token = User.GetLoggedInUserToken();
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/UpdateIsPublishVarAttr", JsonConvert.SerializeObject(req), _token);
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                }
            }
            return Json(res);
        }
        [HttpPost]
        public async Task<IActionResult> VariantAttributeList(int Id)
        {
            var response = new List<ProductAttributes>();
            string _token = User.GetLoggedInUserToken();
            var jsonData = JsonConvert.SerializeObject(new SearchItem { Id = Id });
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/ProductHome/GetProductAttrDetails", jsonData, _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<ProductAttributes>>>(apiResponse.Result);
                response = deserializeObject.Result;
            }
            return PartialView("Partials/_VariantAttributeList", response);
        }

        [HttpPost]
        public async Task<IActionResult> VariantDetail(int Id, string Color = "")
        {
            var response = new VariantDetailVM
            {
                Attributes = new List<ProductAttributes>()
            };
            string _token = User.GetLoggedInUserToken();
            var jsonData = JsonConvert.SerializeObject(new SearchItem { Id = Id });
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/ProductHome/GetProductAttrDetails", jsonData, _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<ProductAttributes>>>(apiResponse.Result);
                response.Attributes = deserializeObject.Result;
            }
            var request = JsonConvert.SerializeObject(new VariantIdByAttributesRequest { VariantId = Id });
            var ResponseDetails = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/ProductHome/GetVariantDetailsByAttributes", request, _token);
            if (ResponseDetails.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<VariantDetailsByAttributesResponse>>(ResponseDetails.Result);
                response.variantDetailsByAttributes = deserializeObject.Result;
            }
            var req = JsonConvert.SerializeObject(new VariantIdByAttributesRequest { VariantId = Id, Color = Color });
            var Responseapi = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/ProductHome/GetVariantPicture", req, _token);
            if (ResponseDetails.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<PictureInformation>>>(Responseapi.Result);
                response.PictureInformation = deserializeObject.Result;
            }
            return PartialView("Partials/_VariantDetail", response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateIsPublishProduct(UpdateIsPublishProduct req)
        {
            var res = new Response();
            if (req.ID >= 1)
            {
                string _token = User.GetLoggedInUserToken();
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/UpdateIsPublishProduct", JsonConvert.SerializeObject(req), _token);
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                }
            }
            return Json(res);
        }
        [Route("Product/GetBrands")]
        [Route("GetBrands")]
        public async Task<IActionResult> GetBrands()
        {
            return Json(await _ddl.GetBrandsDDL(GetToken(), _apiBaseURL));
        }

        [HttpGet("Product/Edit/{Id}")]
        public async Task<IActionResult> Edit(int Id = 0)
        {
            var model = new ProductViewModel();
            model.Categories = await _ddl.GetCategoryDDL(GetToken(), _apiBaseURL);
            model.Brands = await _ddl.GetBrandsDDL(GetToken(), _apiBaseURL);
            var response = new List<Products>();
            if (Id > 0)
            {
                var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/GetProducts", JsonConvert.SerializeObject(new SearchItem { Id = Id }), User.GetLoggedInUserToken());
                if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<List<Products>>>(apiResponse.Result);
                    response = deserializeObject.Result;
                    var product = deserializeObject.Result.FirstOrDefault();
                    model.SKU = product.SKU;
                    model.Name = product.Name;
                    model.Id = product.Id;
                    model.Title = product.Title;
                    model.Description = product.Description;
                    model.BrandId = product.BrandId;
                    model.BrandName = product.BrandName;
                    model.CategoryId = product.CategoryId;
                    model.CategoryName = product.CategoryName;
                    model.ShippingDetailId = product.ShippingDetailId;
                    model.ProductId = product.Id;
                    model.Charges = product.Charges;
                    model.FreeOnAmount = product.FreeOnAmount;
                    model.IsFlat = product.IsFlat;
                    model.IsCod = product.IsCod;
                    model.Specification = product.Specification;
                    model.ShortDescription = product.ShortDescription;
                }
            }
            return View("Partials/_Edit", model);
        }

        [HttpPost]
        [ValidateAjax]
        public async Task<IActionResult> AddProduct(Products model)
        {
            var response = new Response();
            try
            {
                string _token = User.GetLoggedInUserToken();
                var jsonData = JsonConvert.SerializeObject(model);
                var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/AddUpdate", jsonData, _token);
                if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
                    response = deserializeObject;
                }
            }
            catch
            {

            }
            return Json(response);
        }
        #endregion

        #region Add Variants

        // GET: ProductController/Create
        public async Task<IActionResult> AddVariant(int Id = 0, int cId = 0)
        {
            VariantViewModel model = new VariantViewModel()
            {
                ProductId = Id,
                CategoryId = cId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetAttributeSectionView(int Id = 0)
        {
            return PartialView("Partials/_Variants");
        }

        public async Task<IActionResult> AddAttributeGroup()
        {
            return PartialView("Partials/_AddAttributeGroup");
        }

        [HttpPost]
        public async Task<IActionResult> AddAttributes(string combinationId, int CategoryId)
        {
            var model = new ViewVariantCombinationModel
            {
                CombinationId = combinationId,
                CategoryId = CategoryId,
                Attributes = await _ddl.GetCategoryMappedAttributeDDL(GetToken(), _apiBaseURL, CategoryId)
            };
            return PartialView("Partials/_AddAttributes", model);
        }
        [HttpPost]
        [ValidateAjax]
        public async Task<IActionResult> SaveVariants([MinLength(1, ErrorMessage = "Add atleast one Image")] List<PictureInformationReq> req, string jsonObj)
        {

            var model = new VariantCombination();
            model = JsonConvert.DeserializeObject<VariantCombination>(jsonObj ?? "");
            ModelState.Clear();
            TryValidateModel(model);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new Response();
            try
            {
                model.PictureInfo = UploadProductImage(req);
                if (model.PictureInfo.Count() > 0)
                {
                    string _token = User.GetLoggedInUserToken();
                    var jsonData = JsonConvert.SerializeObject(model);
                    var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/AddProductVariant", jsonData, _token);
                    if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
                    {
                        var deserializeObject = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
                        response = deserializeObject;
                    }
                }
                else
                {
                    response.ResponseText = "Image is currupted.Please try another image.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return Json(response);
        }
        //[HttpPost]
        //public async Task<IActionResult> AttributesList()
        //{
        //    var model = new ViewVariantCombinationModel
        //    {
        //        CombinationId = combinationId,
        //        CategoryId = CategoryId,
        //        Attributes = await _ddl.GetCategoryMappedAttributeDDL(GetToken(), _apiBaseURL, CategoryId)
        //    };
        //    return PartialView("Partials/_AddAttributes", model);
        //}
        [HttpPost]
        public async Task<ActionResult> VariantQuantityUpdate(int v, int q, bool IsReduce, string Remark)
        {
            var response = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            if (q <= 0)
            {
                response.ResponseText = "Minimum 1 Quantity Required.";
                return Ok(response);
            }
            if (v <= 0)
            {
                response.ResponseText = "Invalid Variant.";
                return Ok(response);
            }
            if (IsReduce && string.IsNullOrEmpty(Remark))
            {
                response.ResponseText = "Remark Required.";
                return Ok(response);
            }
            string _token = User.GetLoggedInUserToken();
            var jsonData = JsonConvert.SerializeObject(new VariantQuantity { VariantId = v, Quantity = q, IsReduce = IsReduce, Remark = Remark });
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/VariantQuantityUpdate", jsonData, _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
                response = deserializeObject;
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteVariantImage(int VariantId, int ImgId, string ImgPath)
        {
            Response response = new Response();
            var jsonData = JsonConvert.SerializeObject(new DeleteVariantReq { VariantId = VariantId, ImgId = ImgId, ImgPath = ImgPath });
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/DeleteVariantImage", jsonData, User.GetLoggedInUserToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<string>>>(apiResponse.Result);
                response.StatusCode = deserializeObject.StatusCode;
                response.ResponseText = deserializeObject.ResponseText;
                if (response.StatusCode == ResponseStatus.Success)
                {
                    foreach (string str in deserializeObject.Result)
                    {
                        response = Helper.O.DeleteFile(str);
                    }
                }
            }
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> UploadVariantImage(int VariantId, string VariantColor, string ImgAlt)
        {
            var model = new ViewVariantCombinationModel
            {
                VariantId = VariantId,
                VariantColor = VariantColor,
                ImgAlt = ImgAlt
            };
            return PartialView("Partials/_UploadVariantImage", model);
        }
        #endregion

        #region Private Method
        private List<PictureInformation> UploadProductImage(List<PictureInformationReq> req)
        {
            List<PictureInformation> ImageInfo = new List<PictureInformation>();
            if (req != null && req.Any())
            {
                int counter = 0;
                foreach (var item in req)
                {
                    counter++;
                    string fileName = $"{counter.ToString() + DateTime.Now.ToString("ddMMyyyyhhmmssmmm")}.jpeg";
                    Utility.O.UploadFile(new FileUploadModel
                    {
                        file = item.file,
                        FileName = fileName,
                        FilePath = FileDirectories.ProductVariant.Replace("/{0}", string.Empty),
                        IsThumbnailRequired = false,
                    });
                    ImageInfo.Add(new PictureInformation
                    {
                        Title = item.Title,
                        Alt = item.Alt,
                        Color = item.Color,
                        DisplayOrder = item.DisplayOrder,
                        GroupId = item.GroupId,
                        ImgVariant = "default",
                        ImagePath = string.Concat(_httpInfo.AbsoluteURL(), "/", FileDirectories.ProductSuffixDefault, fileName)
                    });
                    foreach (var iSize in _ImageSize)
                    {
                        bool isExists = _ImageSize.TryGetValue(iSize.Key, out string sValue);
                        if (isExists)
                        {
                            var dimension = sValue.Split("_");
                            ImageResizer resizer = new ImageResizer();
                            var resizedImg = resizer.ResizeImage(item.file, Convert.ToInt32(dimension[0]), Convert.ToInt32(dimension[1]));
                            if (resizedImg == null)
                            {
                                ImageInfo = new List<PictureInformation>();
                                goto Finish;
                            }
                            using (var stream = new MemoryStream())
                            {
                                resizedImg.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                var formFile = new FormFile(stream, 0, stream.Length, "req[0].file", fileName)
                                {
                                    Headers = new HeaderDictionary(),
                                    ContentDisposition = "form-data;FileName=" + fileName,
                                    ContentType = "image/jpeg"
                                };
                                string Paths = FileDirectories.ProductVariant.Replace("{0}", sValue);
                                Utility.O.UploadFile(new FileUploadModel
                                {
                                    file = formFile,
                                    FileName = fileName,
                                    FilePath = FileDirectories.ProductVariant.Replace("{0}", sValue),
                                    IsThumbnailRequired = false,
                                });
                                ImageInfo.Add(new PictureInformation
                                {
                                    Title = item.Title,
                                    Alt = item.Alt,
                                    Color = item.Color,
                                    DisplayOrder = item.DisplayOrder,
                                    GroupId = item.GroupId,
                                    ImgVariant = sValue,
                                    ImagePath = string.Concat(_httpInfo.AbsoluteURL(), "/", FileDirectories.ProductSuffix.Replace("{0}", sValue), fileName)
                                });
                            }
                        }
                    }
                }
            }
        Finish:
            return ImageInfo;
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
        #endregion
        #region Rating
        [Route("ProductRating")]
        [HttpPost]
        public async Task<IActionResult> ProductRating(List<ProductRating> req)
        {
            string _token = User.GetLoggedInUserToken();
            var path = UploadRatingImage(req);
            var requ = req.FirstOrDefault();
            var request = new ProductRating
            {
                VariantID = requ.VariantID,
                Title = requ.Title,
                Rating = requ.Rating,
                Review = requ.Review,
                Images = path,
            };
            var jsonData = JsonConvert.SerializeObject(request);
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/ProductRating", jsonData, _token);
            var res = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
            return Json(res);
        }
        private string UploadRatingImage(List<ProductRating> req)
        {
            var ImagePath = new List<string>();
            if (req != null && req.Any())
            {
                int counter = 0;
                foreach (var item in req)
                {
                    counter++;
                    string fileName = $"{counter.ToString() + DateTime.Now.ToString("ddMMyyyyhhmmssmmm")}.jpeg";
                    Utility.O.UploadFile(new FileUploadModel
                    {
                        file = item.file,
                        FileName = fileName,
                        FilePath = FileDirectories.ProductRateSuffixDefault.Replace("/{0}", string.Empty),
                        IsThumbnailRequired = false,
                    });
                    ImagePath.Add(string.Concat(_httpInfo.AbsoluteURL() + "/",FileDirectories.ProductRateSuffixDefault, fileName));
                }
            }
            return string.Join(',',ImagePath);
        }
        #endregion

    }
}
