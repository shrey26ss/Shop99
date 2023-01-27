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
        public async Task<IResponse<IEnumerable<Inventory>>> GetInventoryLadgerReport(RequestBase<InventoryRequest> req)
        {
            var res = new Response<IEnumerable<Inventory>>();
            try
            {
                if (req.RoleId == Convert.ToInt32(Role.Admin))
                {
                    string sp = @"if(@Status = 0)
Select i.*,p.[Name] ProductName,vg.Title VariantTitle from Inventory i(nolock) 
inner join VariantGroup vg(nolock) on vg.Id = i.VarriantId
inner join Products p(nolock) on p.Id = vg.ProductId
Order by i.Id desc
else
Select i.*,p.[Name] ProductName,vg.Title VariantTitle from Inventory i(nolock) 
inner join VariantGroup vg(nolock) on vg.Id = i.VarriantId
inner join Products p(nolock) on p.Id = vg.ProductId
where Qty <=10
Order by i.Id desc";
                    res.Result = await _dapper.GetAllAsync<Inventory>(sp, new { req.Data.Status }, CommandType.Text);
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
        public async Task<IResponse<IEnumerable<Inventory>>> GetInventoryReport(RequestBase<InventoryRequest> req)
        {
            var res = new Response<IEnumerable<Inventory>>();
            try
            {
                if (req.RoleId == Convert.ToInt32(Role.Admin))
                {
                    string sp = @"if(@Status = 0)
Select SUM(i.Qty) Qty,i.Remark,p.[Name] ProductName,vg.Title VariantTitle from Inventory i(nolock) 
inner join VariantGroup vg(nolock) on vg.Id = i.VarriantId
inner join Products p(nolock) on p.Id = vg.ProductId
group by i.Qty,p.[Name],vg.Title,i.Remark
else
Select SUM(i.Qty) Qty,i.Remark,p.[Name] ProductName,vg.Title VariantTitle from Inventory i(nolock) 
inner join VariantGroup vg(nolock) on vg.Id = i.VarriantId
inner join Products p(nolock) on p.Id = vg.ProductId
group by i.Qty,p.[Name],vg.Title,i.Remark
having SUM(i.Qty) <=10";
                    res.Result = await _dapper.GetAllAsync<Inventory>(sp, new { req.Data.Status }, CommandType.Text);
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
