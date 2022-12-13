using AppUtility.Helper;
using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Countries;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Service.State
{
    public class StateService : IStates
    {
        public readonly IDapperRepository _dapper;
        private readonly ILogger<StateService> _logger;
        public StateService(IDapperRepository dapper, ILogger<StateService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse> AddUpdate(States request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Id != 0 && request.Id > 0)
                {
                    sqlQuery = @"Update States Set StateName = @StateName, CountryId=@CountryId where Id = @Id";
                }
                else
                {
                    sqlQuery = @"insert into States(StateName,CountryId,IsActive) values(@StateName,@CountryId,1)";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.Id,
                    request.StateName,
                    request.CountryId
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
                sqlQuery = @"Update States Set IsActive = ISNULL(IsActive,0)^1 where Id = @Id";
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
        public async Task<IResponse<IEnumerable<States>>> GetList()
        {
            string sp = @"Select * from States(nolock) Order by StateName";
            var res = new Response<IEnumerable<States>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<States>(sp, new { }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IEnumerable<StateDDL>> GetDDL()
        {
            string sp = @"Select * from States(nolock) Order by StateName";
            var res = new List<StateDDL>();
            try
            {
                return  await _dapper.GetAllAsync<StateDDL>(sp, new { }, CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
    }
}
