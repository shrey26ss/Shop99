using AppUtility.Helper;
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
using System.Linq;
using System.Threading.Tasks;

namespace Service.CartWishList
{
    public class CartWishListService : ICartWishListService
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public CartWishListService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse> AddWishList(RequestBase<WishList> wishlist)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;

                sqlQuery = @"if not exists(select 1 from  Wishlist (nolock)  where VariantID=@VariantID and EntryBy=@EntryBy)
                            begin
                            insert into Wishlist(VariantID,EntryBy)values(@VariantID,@EntryBy)
                            end
                                else
                                begin
                                update Wishlist set EntryBy=@EntryBy,VariantID=@VariantID  where VariantID=@VariantID and EntryBy=@EntryBy 
                                end
                            ";
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    wishlist.Data.VariantID,
                    wishlist.Data.EntryBy,

                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i >= 0 && i < 10)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = "WishList added successfully";
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
        public async Task<IResponse> AddToCart(RequestBase<CartItem> cartitem)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;

                sqlQuery = @"if not exists(select 1 from CartItem where UserID=@UserID and VariantID=@VariantID)
                            begin
                            insert into CartItem(UserID,VariantID,Qty,EntryOn)values(@UserID,@VariantID,@Qty,GETDATE())
                            end
                            else
                            begin
                            update CartItem set Qty=Qty+1 where UserID=@UserID and VariantID=@VariantID
                            end";
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    cartitem.Data.UserID,
                    cartitem.Data.VariantID,
                    cartitem.Data.Qty,

                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i >= 0 && i < 10)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = "Cart added successfully";
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
        public async Task<IResponse<IEnumerable<CartItems>>> GetCartList(Request req)
        {
            string sp = @"Select w.Id CartId, v.Id VariantID, v.MRP, v.SellingCost, v.Title, v.Thumbnail ImagePath  from Wishlist w inner join VariantGroup v on v.Id = w.VariantID where w.EntryBy = @LoginId";
            var res = new Response<IEnumerable<CartItems>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<CartItems>(sp, new { req.LoginId }, CommandType.Text);
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
