using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Models;
using Service.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Report
{
    public class ReportService : IReport
    {
        private IDapperRepository _dapper;
        private readonly ILogger<ReportService> _logger;
        public ReportService(IDapperRepository dapper, ILogger<ReportService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse<IEnumerable<Inventory>>> GetInventoryReport(Request req)
        {
            var res = new Response<IEnumerable<Inventory>>();
            try
            {
                if (req.RoleId == Convert.ToInt32(Role.Admin))
                {
                    string sp = @"Select i.*,p.[Name] ProductName,vg.Title VariantTitle from Inventory i(nolock) 
inner join VariantGroup vg(nolock) on vg.Id = i.VarriantId
inner join Products p(nolock) on p.Id = vg.ProductId
Order by i.Id desc";
                    res.Result = await _dapper.GetAllAsync<Inventory>(sp, null, CommandType.Text);
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = nameof(ResponseStatus.Success);
                }
                else
                {
                    res.ResponseText = "Unauthorised";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public Task<IResponse<ReportRow>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<IEnumerable<ReportColumn>>> GetAsync(int loginId = 0, dynamic param = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<IEnumerable<TColumn>>> GetAsync<TColumn>(int loginId = 0, Expression<Func<TColumn, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse> AddAsync(RequestBase<ReportRow> entity)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
