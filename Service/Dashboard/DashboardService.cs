using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Models;
using Service.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dashboard
{
    public class DashboardService : IDashboard
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DashboardService> _logger;
        public DashboardService(IDapperRepository dapper, ILogger<DashboardService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse<DashboardTopBoxCount>> GetDashboardTopBoxCount(Request req)
        {
            var res = new Response<DashboardTopBoxCount>();
            try
            {
                if (req.RoleId == Convert.ToInt32(Role.ADMIN))
                {
                    string sp = @"declare @LowStocks int=0
Select (SELECT Count(1) FROM Orders(nolock) where StatusID = 1) TotalOrdersPlaced, (SELECT Count(1) FROM Orders(nolock) WHERE StatusID = 3) ConfirmedOrder,(Select Distinct Count(Id) from VariantGroup where Quantity <= 10) LowStocks, (SELECT Count(1) FROM Users u(nolock) INNER JOIN UserRoles ur(nolock) on ur.UserId = u.Id WHERE ur.RoleId = 2) TotalCustomer,(select Count(Id) from VariantGroup(nolock) where ISNULL(AdminApproveStatus,'') = '') TotalPendingApprovel";
                    res.Result = await _dapper.GetAsync<DashboardTopBoxCount>(sp, null, CommandType.Text);
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
    }
}
