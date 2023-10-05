using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Product
{
    public class ProductHomeService : IProductHomeService
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public ProductHomeService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse<ProductDetails>> GetProductDetails(SearchItem req)
        {
            string sp = "Proc_GetProductDetails";
            var res = new Response<ProductDetails>();
            try
            {
         
                res.Result = await _dapper.GetAsync<ProductDetails>(sp, new { req.Id,req.UserID }, CommandType.StoredProcedure);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                
            }
            return res;
        }
        public async Task<IResponse<ProductDetails>> GetProductAllDetails(SearchItem req)
        {
            string sp = "Proc_GetProductDetails";
            var res = new Response<ProductDetails>();
            try
            {
                var productdetails = await _dapper.GetAsync<ProductDetails>(sp, new { req.Id, req.UserID }, CommandType.StoredProcedure);
                var picInfoList = JsonConvert.DeserializeObject<List<ProductPictureInfo>>(productdetails?.Images ?? "");
                var AttributeInfo = await _dapper.GetAllAsync<AttributeInfo>("Proc_ProductAttrInfo", new { VariantId = req.Id }, CommandType.StoredProcedure);
                var AttributDetails = await _dapper.GetAllAsync<ProductAttributes>("Proc_ProductAttrDetails", new { VariantId = req.Id }, CommandType.StoredProcedure);
                var offer = await _dapper.GetAllAsync<specialoffer>("Proc_GetOffers", new { VariantId = req.Id }, CommandType.StoredProcedure);
                var coupons = await _dapper.GetAllAsync<Coupon>("Proc_GetCouponForVariant", new { VariantId = req.Id }, CommandType.StoredProcedure);
                productdetails = productdetails ?? new ProductDetails();
                productdetails.sepcialoffer = offer.ToList();
                productdetails.Coupons = coupons.ToList();
                productdetails.ProductPictureInfos = picInfoList;
                productdetails.AttributeInfo = AttributeInfo.ToList();
                productdetails.ProductAttributes = AttributDetails.ToList();
                res.Result = productdetails;
                if(res.Result.ProductId == 0)
                {
                    res.StatusCode = ResponseStatus.Failed;
                    res.ResponseText = nameof(ResponseStatus.Failed);
                }
                else
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = nameof(ResponseStatus.Success);
                }
                
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<IResponse<IEnumerable<ProductAttributes>>> GetProductAttrDetails(SearchItem req)
        {
            string sp = "Proc_ProductAttrDetails";
            var res = new Response<IEnumerable<ProductAttributes>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<ProductAttributes>(sp, new { VariantId = req.Id }, CommandType.StoredProcedure);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public async Task<IResponse<IEnumerable<AttributeInfo>>> GetProductAttributeInfo(SearchItem req)
        {
            string sp = "Proc_ProductAttrInfo";
            var res = new Response<IEnumerable<AttributeInfo>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<AttributeInfo>(sp, new { VariantId = req.Id,req.UserID }, CommandType.StoredProcedure);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public async Task<IResponse<IEnumerable<ProductPictureInfo>>> GetProductPicDetails(SearchItem req)
        {
            string sp = "Proc_GetProductPicDetails";
            var res = new Response<IEnumerable<ProductPictureInfo>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<ProductPictureInfo>(sp, new { variantId = req.Id }, CommandType.StoredProcedure);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<IResponse<VariantIdByAttributesResponse>> GetVariantIdByAttributes(RequestBase<VariantIdByAttributesRequest> request)
        {
            var res = new Response<VariantIdByAttributesResponse>();
            if (request.Data != null)
            {
                try
                {
                    string sp = @"proc_GetMoreProduct";
                    res.Result = await _dapper.GetAsync<VariantIdByAttributesResponse>(sp, new { request.Data.VariantId, request.Data.Attributes }, CommandType.StoredProcedure);
                    if (res.Result != null && res.Result.VariantId > 0)
                    {
                        res.StatusCode = ResponseStatus.Success;
                        res.ResponseText = ResponseStatus.Success.ToString();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return res;
        }
        public async Task<IResponse<VariantDetailsByAttributesResponse>> GetVariantdetailsByAttributes(RequestBase<VariantIdByAttributesRequest> request)
        {
            var res = new Response<VariantDetailsByAttributesResponse>();
            if (request.Data != null)
            {
                try
                {
                    string sp = @"select v.*, p.CategoryId from VariantGroup v(nolock) inner join Products p(nolock) on p.Id = v.ProductId where v.Id = @VariantId";
                    res.Result = await _dapper.GetAsync<VariantDetailsByAttributesResponse>(sp, new { request.Data.VariantId }, CommandType.Text);
                    if (res.Result != null && res.Result.Id > 0)
                    {
                        res.StatusCode = ResponseStatus.Success;
                        res.ResponseText = ResponseStatus.Success.ToString();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<PictureInformation>>> PictureInformation(RequestBase<VariantIdByAttributesRequest> request)
        {
            var res = new Response<IEnumerable<PictureInformation>>();
            if (request.Data != null)
            {
                try
                {
                    string picInfo = await _dapper.GetAsync<string>("Select Images from VariantGroup where Id = @VariantId", new { request.Data.VariantId}, CommandType.Text);
                    res.Result = JsonConvert.DeserializeObject<IEnumerable<PictureInformation>>(picInfo);
                    res.Result = res.Result.Where(a => a.ImgVariant == "default");
                    if (res.Result != null)
                    {
                        res.StatusCode = ResponseStatus.Success;
                        res.ResponseText = ResponseStatus.Success.ToString();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return res;
        }
    }
}
