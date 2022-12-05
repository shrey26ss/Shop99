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
            string sp = @"Select ai.AttributeId AttributeId, a.[Name] AttributeName, ai.AttributeValue from AttributeInfo ai inner join Attributes a on a.Id = ai.AttributeId where GroupId = @Id";
            var res = new Response<IEnumerable<ProductAttributes>> ();
            try
            {
                res.Result = await _dapper.GetAllAsync<ProductAttributes>(sp, new { req.Id }, CommandType.Text);
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
            string sp = @"Select Color, Title, Alt,ImagePath,ImgVariant from PictureInformation where GroupId = @Id";
            var res = new Response<IEnumerable<ProductPictureInfo>> ();
            try
            {
                res.Result = await _dapper.GetAllAsync<ProductPictureInfo>(sp, new { req.Id }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
