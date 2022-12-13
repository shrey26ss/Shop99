
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

namespace Service.Countries
{
    public class CountryService : ICountry
    {
        public readonly IDapperRepository _dapper;
        private readonly ILogger<CountryService> _logger;
        public CountryService(IDapperRepository dapper, ILogger<CountryService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse> AddUpdate(Country request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Id != 0 && request.Id > 0)
                {
                    sqlQuery = @"Update Country Set CountryName = @CountryName where Id = @Id";
                }
                else
                {
                    sqlQuery = @"Insert into Country(CountryName,IsActive) values(@CountryName,1)";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.Id,
                    request.CountryName
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
                sqlQuery = @"Update Country Set IsActive = ISNULL(IsActive,0)^1 where Id = @Id";
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
        public async Task<IResponse<IEnumerable<Country>>> GetList()
        {
            string sp = @"Select * from Country(nolock) order by CountryName";
            var res = new Response<IEnumerable<Country>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<Country>(sp, new { }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
            }
            return res;
        }
    }
}
