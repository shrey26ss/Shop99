using AppUtility.Helper;
using Data;
using Data.Models;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace Service.Offers
{
    public class OffersService : IOffersService
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public OffersService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse> AddUpdateOffer(RequestBase<GetOffers> offers)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                if (offers.Data.OfferId != 0 && offers.Data.OfferId > 0)
                {
                    sqlQuery = @"update Offers set OfferTitle = @OfferTitle,OfferDescription=@OfferDescription,AtRate=@AtRate,OfferTypeId = @OfferTypeId,IsActive=@IsActive where OfferId = @OfferId;Select 1 StatusCode,'Offer Updated successfully' ResponseText";
                }
                else
                {
                    sqlQuery = @"Insert into Offers (OfferTypeId,OfferTitle,OfferDescription,OfferBeginOn,OfferEndOn,IsFlat,AtRate,DiscountUpToAmount,MinEligibleAmount,IsActive,	UserID,IsAutoApply,CouponCode) values(@OfferTypeId,@OfferTitle,@OfferDescription,GETDATE(),GETDATE(),@Isflat,@AtRate,@DiscountUpToAmount,@MinEligibleAmount,@IsActive,@LoginId,@IsAutoApply,@CouponCode);Select 1 StatusCode,'Offer Inserted successfully' ResponseText";
                }
                res = await _dapper.GetAsync<Response>(sqlQuery, new
                {
                    offers.Data.OfferId,
                    offers.Data.OfferTypeId,
                    OfferTitle = offers.Data.OfferTitle ?? String.Empty,
                    OfferDescription = offers.Data.OfferDescription ?? String.Empty,
                    Isflat = true,
                    offers.Data.AtRate,
                    offers.Data.DiscountUpToAmount,
                    offers.Data.MinEligibleAmount,
                    offers.Data.IsActive,
                    offers.LoginId,
                    IsAutoApply = true,
                    CouponCode = offers.Data.CouponCode ?? String.Empty
                }, CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<GetOffers>>> Getoffers(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<GetOffers>>();
            try
            {
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"select * from Offers where OfferId = @Id";
                    res.Result = await _dapper.GetAllAsync<GetOffers>(sp, new { request.Data.Id }, CommandType.Text);
                }
                else
                {
                    //sp = @"Select c.*, p.CategoryName as ParentName from Category(nolock) c inner join Category p on p.CategoryId = c.ParentId Order by c.Ind";
                    sp = @"select * from Offers";
                    res.Result = await _dapper.GetAllAsync<GetOffers>(sp, new { }, CommandType.Text);
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
        public async Task<IResponse<IEnumerable<offerstype>>> GetOfferDDL()
        {
            string sp = @"select * from Offertype";
            var res = new Response<IEnumerable<offerstype>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<offerstype>(sp, new { }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        //public async Task<IResponse> UpdateIsActiveOffer(RequestBase<OfferUpdateIsActive> offer)
        //{
        //    var res = new Response();
        //    try
        //    {
        //        string sqlQuery = "";
        //        int i = -5;

        //        sqlQuery = @"update offers set IsActive = @IsActive where OfferID = @OfferID;";

        //        i = await _dapper.ExecuteAsync(sqlQuery, new
        //        {
        //            offer.Data.OfferID,
        //            offer.Data.IsActive
        //        }, CommandType.Text);
        //        var description = Utility.O.GetErrorDescription(i);
        //        if (i > 0 && i < 10)
        //        {
        //            res.StatusCode = ResponseStatus.Success;
        //            res.ResponseText = "Offer Updated successfully";
        //        }
        //        else
        //        {
        //            res.ResponseText = description;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //    }
        //    return res;
        //}
        public async Task<IResponse> UpdateIsActiveOffer(RequestBase<OfferUpdateIsActive> offer)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                sqlQuery = @"update offers set IsActive = @IsActive where OfferID = @OfferID;Select 1 StatusCode,'Offer Updated successfully' ResponseText";
                res = await _dapper.GetAsync<Response>(sqlQuery, new
                {
                    offer.Data.OfferID,
                    offer.Data.IsActive
                }, CommandType.Text);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<Coupon>>> GetCoupons(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<Coupon>>();
            try
            {
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"select CouponId,CouponCode	,IsFixed	,DiscountAmount	,convert(varchar,EntryOn,106) EntryOn	,IsActive	,PaymentModes	,IsWelcomeCoupon	,[Description]	,convert(varchar,ExpiryOn,106) ExpiryOn, MaxBenefit,UseCount,IsProductDependent,MinPurchaseForRedeem from Coupon where CouponId = @CouponId";
                    res.Result = await _dapper.GetAllAsync<Coupon>(sp, new {CouponId = request.Data.Id }, CommandType.Text);
                }
                else
                {
                    
                    sp = @"select CouponId,CouponCode	,IsFixed	,DiscountAmount	,convert(varchar,EntryOn,106) EntryOn	,IsActive,PaymentModes	,IsWelcomeCoupon	,[Description]	,convert(varchar,ExpiryOn,106) ExpiryOn,MaxBenefit,UseCount,IsProductDependent,MinPurchaseForRedeem from Coupon order by CouponId";
                    res.Result = await _dapper.GetAllAsync<Coupon>(sp, new { }, CommandType.Text);
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

        public async Task<IResponse> AddUpdateCoupon(RequestBase<Coupon> coupon)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                if (coupon.Data.CouponId != 0 && coupon.Data.CouponId > 0)
                {
                    sqlQuery = @"IF EXISTS(Select 1 FROM Coupon Where CouponCode=@CouponCode AND CouponId<>@CouponId)
                                 BEGIN
                                   SELECT -1 StatusCode,'Coupon Already Exists !' ResponseText
                                 END



                              update Coupon Set CouponCode=@CouponCode , IsFixed =@IsFixed , DiscountAmount=@DiscountAmount ,ExpiryOn=@ExpiryOn , IsActive=@IsActive,PaymentModes=@PaymentModes,IsWelcomeCoupon =@IsWelcomeCoupon,Description=@Description,MaxBenefit=@MaxBenefit,UseCount=@UseCount,IsProductDependent=@IsProductDependent,MinPurchaseForRedeem=@MinPurchaseForRedeem
                        where CouponId = @CouponId;Select 1 StatusCode,'Coupon Updated successfully' ResponseText";
                }
                else
                {
                    sqlQuery = @"IF EXISTS(Select 1 FROM Coupon Where CouponCode=@CouponCode)
                                 BEGIN
                                   SELECT -1 StatusCode,'Coupon Already Exists !' ResponseText
                                 END
                        Insert into Coupon(CouponCode , IsFixed , DiscountAmount , EntryOn, IsActive,PaymentModes,IsWelcomeCoupon,Description,ExpiryOn ,MaxBenefit,UseCount,IsProductDependent,MinPurchaseForRedeem)
                                       Values(@CouponCode , @IsFixed , @DiscountAmount , Getdate(),@IsActive,@PaymentModes,@IsWelcomeCoupon,@Description,@ExpiryOn,@MaxBenefit,@UseCount,@IsProductDependent,@MinPurchaseForRedeem);select 1 StatusCode,'Coupon Inserted successfully' ResponseText";
                }
                res = await _dapper.GetAsync<Response>(sqlQuery, new
                {
                    coupon.Data.CouponId,
                    coupon.Data.CouponCode,
                    coupon.Data.IsFixed,
                    coupon.Data.DiscountAmount,
                    coupon.Data.IsActive,
                    coupon.Data.Description,
                    coupon.Data.PaymentModes,
                    coupon.Data.IsWelcomeCoupon,
                    coupon.Data.ExpiryOn,
                    coupon.Data.IsProductDependent,
                    coupon.Data.MaxBenefit,
                    coupon.Data.MinPurchaseForRedeem,
                    coupon.Data.UseCount
                },
                CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse> UpdateIsActiveCoupon(RequestBase<CouponUpdateIsActive> coupon)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                sqlQuery = @"update Coupon set IsActive = @IsActive ,IsFixed = @IsFixed, IsWelcomeCoupon= @IsWelcomeCoupon ,IsProductDependent = @IsProductDependent  where CouponId = @CouponId;Select 1 StatusCode,'Coupon Updated successfully' ResponseText";
                res = await _dapper.GetAsync<Response>(sqlQuery, new
                {
                    coupon.Data.CouponId,
                    coupon.Data.IsActive,
                    coupon.Data.IsWelcomeCoupon,
                    coupon.Data.IsFixed,
                    coupon.Data.IsProductDependent
                }, CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public async Task<IResponse> DelCoupon(RequestBase<Coupon> coupon)
        {
            var res = new Response();
            try
            {
                if (coupon.Data.CouponId != null && coupon.Data.CouponId > 0)
                {
                    string sqlQuery = @"Delete from Coupon where CouponId=@CouponId";
                    var res1 = await _dapper.ExecuteAsync(sqlQuery, new { coupon.Data.CouponId }, CommandType.Text);
                    if (res1 != null)
                    {
                        res.StatusCode = ResponseStatus.Success;
                        res.ResponseText = "Coupon Removed successfully";
                    }
                    else
                    {
                        res.StatusCode = ResponseStatus.Failed;
                        res.ResponseText = "Can't delete coupon";
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<Coupon>>> GetCartProductCoupons(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<Coupon>>();
            try
            {
                if (request.LoginId > 0)
                {
                    sp = @"Select * from (
                   Select tempc.* FROM coupon tempc
				   LEFT JOIN  VariantGroup  vg on VG.CouponId=tempc.CouponId
				   LEFT JOIN  CartItem  c on VG.Id=c.VariantID
				   AND C.UserID=@LoginId) tt Where 
				   tt.ExpiryOn > GETDATE()
				   AND ISNULL(tt.IsActive,0)=1";
                    res.Result = await _dapper.GetAllAsync<Coupon>(sp, new { request.LoginId }, CommandType.Text);
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
    }
}
