using Data;
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

        //public async Task<IResponse<IEnumerable<UserDetails>>> GetUserListByRole(Role id)
        //{
        //    string sp = @"select Name,UserName,PhoneNumber,Email,* from users u inner join UserRoles ur on  u.ID=ur.UserId 
        //            where ur.RoleId =@ID order by u.Id desc";
        //    var res = new Response<IEnumerable<UserDetails>>();
        //    try
        //    {
        //        res.Result = await _dapper.GetAllAsync<UserDetails>(sp, new { ID = (int)id }, CommandType.Text);
        //        res.StatusCode = ResponseStatus.Success;
        //        res.ResponseText = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //    }
        //    return res;
        //}
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


    }
}
