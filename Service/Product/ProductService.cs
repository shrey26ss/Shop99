using AppUtility.Helper;
using Dapper;
using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using NLog;
using Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Product
{
    public class ProductService : IProducts
    {
        private IDapperRepository _dapper;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IDapperRepository dapper, ILogger<ProductService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task<IResponse> AddUpdate(RequestBase<Products> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "Proc_AddUpdateProductAndShippingDetails";
                res = await _dapper.GetAsync<Response>(sqlQuery, new
                {
                    request.LoginId,
                    request.Data.Id,
                    request.Data.Name,
                    request.Data.Title,
                    Description = request.Data.Description ?? String.Empty,
                    SKU = request.Data.SKU ?? string.Empty,
                    request.Data.BrandId,
                    request.Data.CategoryId,
                    request.Data.IsFlat,
                    request.Data.Charges,
                    request.Data.FreeOnAmount,
                    request.Data.ShippingDetailId,
                    Specification = request.Data.Specification ?? String.Empty,
                    request.Data.ShortDescription,
                    request.Data.IsCod,
                }, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<Products>>> GetProducts(RequestBase<ProductSearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new ProductSearchItem();
            var res = new Response<IEnumerable<Products>>();
            try
            {
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"Select p.*, c.CategoryName,b.[Name] BrandName, s.Charges,s.FreeOnAmount,s.IsFlat,s.Id ShippingDetailId from Products p 
inner join Category c on c.CategoryId = p.CategoryId 
inner join Brands b on b.Id = p.BrandId 
inner join ProductShippingDetail s on s.ProductId = p.Id
where p.Id = @Id";
                    res.Result = await _dapper.GetAllAsync<Products>(sp, new { request.Data.Id, request.LoginId }, CommandType.Text);
                }
                else
                {
                    sp = @"Select p.*, c.CategoryName,b.[Name] BrandName, s.Charges,s.FreeOnAmount,s.IsFlat,s.Id ShippingDetailId from Products p 
inner join Category c on c.CategoryId = p.CategoryId 
inner join Brands b on b.Id = p.BrandId 
inner join ProductShippingDetail s on s.ProductId = p.Id where (@CategoryID=0 or p.CategoryId=@CategoryID) and p.Name like '%'+@SearchText+'%' Order By p.Id desc";
                    res.Result = await _dapper.GetAllAsync<Products>(sp, new { request.Data.CategoryID, SearchText = request.Data.SearchText ?? string.Empty }, CommandType.Text);
                }
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse> UpdateIsPublishProduct(RequestBase<UpdateIsPublishProduct> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;

                sqlQuery = @"Update Products set IsPublished = @IsPublish where Id = @ID;";

                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.Data.ID,
                    request.Data.IsPublish
                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = "Product Updated successfully";
                }
                else
                {
                    res.ResponseText = description;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse> UpdateIsPublishVarAttr(RequestBase<UpdateIsPublishProduct> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;

                sqlQuery = @"Update VariantGroup set IsPublished = @IsPublish where Id = @ID;";

                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.Data.ID,
                    request.Data.IsPublish
                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = "Variant Updated successfully";
                }
                else
                {
                    res.ResponseText = description;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse> AddProductVariant(RequestBase<VariantCombination> request)
        {
            var res = new Response();
            try
            {
                var VariantGroup = ConvertToDataTable.ToDataTable(request.Data.GroupInfo);
                var AttributeInfo = ConvertToDataTable.ToDataTable(request.Data.AttributeInfo);
                var PictureInfo = ConvertToDataTable.ToDataTable(request.Data.PictureInfo);
                var picturejson = request.Data.PictureInfo.ToString();
                string sqlQuery = "Proc_AddVariant";
                int i = -5;
                DynamicParameters param = new DynamicParameters();
                param.Add("VariantGroup", VariantGroup, DbType.Object);
                param.Add("AttributeInfo", AttributeInfo, DbType.Object);
                param.Add("PictureInfo", PictureInfo, DbType.Object);
                param.Add("ProductId", request.Data.ProductId, DbType.Int32);
                param.Add("EntryBy", request.LoginId, DbType.Int32);
                param.Add("PictureInfojson", picturejson, DbType.String);
                i = await _dapper.GetByDynamicParamAsync<int>(sqlQuery, param, CommandType.StoredProcedure);
                if (i > -1 && i < 100)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = ResponseStatus.Success.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return res;
        }

        public async Task<IResponse> VariantQuantityUpdate(RequestBase<VariantQuantity> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = @"	declare @OpeningQty int,@ClosingQty int,@VQuantity int
                                         select  @VQuantity=Quantity from VariantGroup  where Id = @VarriantId
	                                     if(@IsReduce=0)
	                                     begin
	                                      update VariantGroup set @ClosingQty=Quantity=Quantity+@Quantity,@OpeningQty=Quantity where Id = @VarriantId
                                                                        insert into[Inventory] (VarriantId, IsOut, OpeningQty, Qty, ClosingQty, Remark, EntryOn, RefferenceId)
                                                                        Select @VarriantId,0,@OpeningQty,@Quantity,@ClosingQty,@Remark,getdate(),@VarriantId	 
	                                     end
	                                     else
	                                     begin
	                                     if((@VQuantity-@Quantity)>=0)
	                                     begin
	                                       update VariantGroup set @ClosingQty=Quantity =Quantity-@Quantity,@OpeningQty=Quantity where Id = @VarriantId
                                                                        insert into[Inventory] (VarriantId, IsOut, OpeningQty, Qty, ClosingQty, Remark, EntryOn, RefferenceId)
                                                                        Select @VarriantId,1,@OpeningQty,@Quantity,@ClosingQty,@Remark,getdate(),@VarriantId
	                                     end
                                         end   ";
                int i = -5;
                DynamicParameters param = new DynamicParameters();
                param.Add("VarriantId", request.Data.VariantId, DbType.Int32);
                param.Add("Quantity", request.Data.Quantity, DbType.Int32);
                param.Add("IsReduce", request.Data.IsReduce, DbType.Boolean);
                param.Add("Remark", request.Data.Remark ?? string.Empty, DbType.String);
                i = await _dapper.GetByDynamicParamAsync<int>(sqlQuery, param, CommandType.Text);
                if (i > -1 && i < 100)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = ResponseStatus.Success.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return res;
        }

        public async Task<IResponse<IEnumerable<ProductVariantAttributeDetails>>> GetProductVarAttrDetails(SearchItem req)
        {
            string sp = @"Select * from VariantGroup where ProductId = @Id";
            var res = new Response<IEnumerable<ProductVariantAttributeDetails>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<ProductVariantAttributeDetails>(sp, new { req.Id }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse<List<string>>> DeletevariantImage(RequestBase<DeleteVariantReq> request)
        {
            var arr = request.Data.ImgPath.Split('/');
            int count = arr.Length;
            string imgName = arr[count - 1];
            var res = new Response<List<string>>();
            try
            {
                string sqlQuery = @"Select ImagePath from PictureInformation(nolock) Where GroupId = @VariantId and ImagePath like '%'+@imgName+'%';
                                    delete from PictureInformation Where GroupId = @VariantId and ImagePath like '%'+@imgName+'%';";
                var response = await _dapper.GetAllAsync<string>(sqlQuery, new
                {
                    request.Data.VariantId,
                    request.Data.ImgId,
                    imgName
                }, CommandType.Text);
                if (response != null && response.Count() > 0)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.Result = response.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse> ProductRating(RequestBase<ProductRatingReq> request)
        {
            string sp = @"ProcReview";

            var res = new Response();
            try
            {
                res = await _dapper.GetAsync<Response>(sp, new
                {
                    request.Data.VariantID,
                    UserID = request.LoginId,
                    request.Data.Title,
                    request.Data.Reting,
                    request.Data.Review,
                    request.Data.Images,
                }, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
