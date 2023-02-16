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
                    sqlQuery = @"update Offers set OfferTitle = @OfferTitle,OfferDescription=@OfferDescription,DiscountUpToAmount=@DiscountUpToAmount,OfferTypeId = @OfferTypeId,IsActive=@IsActive where OfferId = @OfferId;Select 1 StatusCode,'Offer Updated successfully' ResponseText";
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
                    Isflat= true,
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
    }
}
