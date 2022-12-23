﻿using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Service.Variant
{
    public class UserRepo : IUserRepo
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public UserRepo(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse<IEnumerable<UserDetails>>> GetUserListByRole(Role id)
        {
            string sp = @"select Name,UserName,PhoneNumber,Email,* from users u inner join UserRoles ur on  u.ID=ur.UserId where ur.RoleId =@ID order by u.Id desc";
            var res = new Response<IEnumerable<UserDetails>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<UserDetails>(sp, new { ID = (int)id }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<VendorProfileList>>> GetVendorList(VendorProfileRequest request = null)
        {
            string sp = @"Select u.Id UserId,u.ConcurrencyStamp,u.Email,u.NormalizedEmail,u.PasswordHash,u.PhoneNumber,u.UserName,u.RefreshToken,u.[Name],u.IsActive, v.ShopName,v.GSTNumber,v.TIN,v.[Address],v.ContactNo,v.StateId,s.StateName [State] 
from Users u(nolock) 
inner join VendorProfile v(nolock) on v.UserId = u.Id 
inner join UserRoles ur(nolock) on ur.UserId = u.Id
inner join States s(nolock) on s.Id = v.StateId
where ur.RoleId = 3";
            var res = new Response<IEnumerable<VendorProfileList>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<VendorProfileList>(sp, null, CommandType.Text);
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
