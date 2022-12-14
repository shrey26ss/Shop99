using AppUtility.Helper;
using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Models;
using Service.State;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Service.City
{
    public class CityService : ICity
    {
        public readonly IDapperRepository _dapper;
        private readonly ILogger<CityService> _logger;
        public CityService(IDapperRepository dapper, ILogger<CityService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse> AddUpdate(Cities request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Id != 0 && request.Id > 0)
                {
                    sqlQuery = @"Update City Set CityName=@CityName,StateId=@StateId where Id = @Id";
                }
                else
                {
                    sqlQuery = @"Insert into City (CityName,StateId,IsActive) values(@CityName,@StateId,1)";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.Id,
                    request.CityName,
                    request.StateId
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
        public async Task<IResponse> ChangeStatus(SearchItem request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                sqlQuery = @"Update City Set IsActive = ISNULL(IsActive,0)^1 where Id = @Id";
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.Id
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
        public async Task<IResponse<IEnumerable<Cities>>> GetList()
        {
            string sp = @"Select * from City(nolock) order by CityName";
            var res = new Response<IEnumerable<Cities>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<Cities>(sp, new { }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IEnumerable<CityDDL>> GetDDL()
        {
            string sp = @"Select * from City(nolock) order by CityName";
            var res = new List<CityDDL>();
            try
            {
                return await _dapper.GetAllAsync<CityDDL>(sp, new { }, CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
    }
}
