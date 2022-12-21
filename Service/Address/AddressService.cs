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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Address
{
    public class AddressService : IRepository<AddressRow, AddressColumn>
    {
        public readonly IDapperRepository _dapper;
        public readonly ILogger<UserAddressService> _logger;
        public AddressService(IDapperRepository dapper, ILogger<UserAddressService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task<IResponse> AddAsync(RequestBase<AddressRow> request)
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
                    sqlQuery = @"Insert into UserAddress (FullName,MobileNo,Pincode,HouseNo,Area,Landmark,TownCity,StateID,AddressTypeID,IsActive,UserID,IsDefault)values (@FullName,@MobileNo,@Pincode,@HouseNo,@Area,@Landmark,@TownCity,@StateID,@AddressTypeID,1,@LoginId,0)";
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

        public Task<IResponse> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<IEnumerable<AddressColumn>>> GetAsync(int loginId = 0)
        {
            string sp = @"Select * from UserAddress(nolock)";
            var res = new Response<IEnumerable<AddressColumn>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<AddressColumn>(sp, new { }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public Task<IResponse<IEnumerable<TColumn>>> GetAsync<TColumn>(int loginId = 0, Expression<Func<TColumn, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<AddressRow>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
