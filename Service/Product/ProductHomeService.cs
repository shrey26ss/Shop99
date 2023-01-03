using Data;
using Data.Models;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
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
                res.Result = await _dapper.GetAsync<ProductDetails>(sp, new { req.Id }, CommandType.StoredProcedure);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<IResponse<IEnumerable<ProductAttributes>>> GetProductAttrDetails(SearchItem req)
        {
            string sp = "Proc_ProductAttrDetails";
            var res = new Response<IEnumerable<ProductAttributes>> ();
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
                res.Result = await _dapper.GetAllAsync<AttributeInfo>(sp, new { VariantId = req.Id }, CommandType.StoredProcedure);
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
            var res = new Response<IEnumerable<ProductPictureInfo>> ();
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
            if (request.Data!=null)
            {
                try
                {
                    string sp = @"proc_GetMoreProduct";
                    res.Result = await _dapper.GetAsync<VariantIdByAttributesResponse>(sp, new { request.Data.VariantId, request.Data.Attributes }, CommandType.StoredProcedure);
                    if (res.Result!=null && res.Result.VariantId > 0)
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
                    string sp = @"select * from VariantGroup where Id = @VariantId";
                    res.Result = await _dapper.GetAsync<VariantDetailsByAttributesResponse>(sp, new { request.Data.VariantId}, CommandType.Text);
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
                    string sp = @"select * from PictureInformation  where GroupId = @VariantId and ImgVariant = 'default'";
                    res.Result = await _dapper.GetAllAsync<PictureInformation>(sp, new { request.Data.VariantId, request .Data.Color}, CommandType.Text);
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
