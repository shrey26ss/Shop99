using AppUtility.Helper;
using Data;
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

namespace Service.Address
{
    public class UserAddressService : IUserAddress
    {
        public readonly IDapperRepository _dapper;
        public readonly ILogger<UserAddressService> _logger;
        public UserAddressService(IDapperRepository dapper, ILogger<UserAddressService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse> AddUpdate(RequestBase<UserAddress> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sqlQuery = @"Update UserAddress Set FullName=@FullName,MobileNo=@MobileNo,Pincode=@Pincode,HouseNo=@HouseNo,Area=@Area,Landmark=@Landmark,TownCity=@TownCity,StateID=@StateID,AddressTypeID=@AddressTypeID,UserID=@LoginId where Id =@Id";
                }
                else
                {
                    sqlQuery = @"
update UserAddress set IsDefault=0 where UserID=@LoginId
Insert into UserAddress (FullName,MobileNo,Pincode,HouseNo,Area,Landmark,TownCity,StateID,AddressTypeID,IsActive,UserID,IsDefault)values (@FullName,@MobileNo,@Pincode,@HouseNo,@Area,@Landmark,@TownCity,@StateID,@AddressTypeID,1,@LoginId,1)";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.LoginId,
                    request.Data.Id,
                    request.Data.FullName,
                    request.Data.MobileNo,
                    request.Data.Pincode,
                    request.Data.HouseNo,
                    request.Data.Area,
                    request.Data.Landmark,
                    request.Data.TownCity,
                    request.Data.StateID,
                    request.Data.AddressTypeID
                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = ResponseStatus.Success.ToString();
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
        public async Task<IResponse<IEnumerable<UserAddress>>> GetList(Request request)
        {
            string sp = @"Select ua.*,s.StateName StateName ,a.AddressName AddressType  from UserAddress(nolock) ua inner join States s on s.Id = ua.StateID inner join AddressType a on a.Id = ua.AddressTypeID where ua.UserID = @LoginID";
            var res = new Response<IEnumerable<UserAddress>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<UserAddress>(sp, new { request.LoginId }, CommandType.Text);
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
