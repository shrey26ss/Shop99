﻿using Data;
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
                if (req.RoleId == Convert.ToInt32(Role.Admin))
                {
                    string sp = @"Select (SELECT Count(1) FROM Orders(nolock)) TotalOrders, (SELECT Count(1) FROM Orders(nolock) WHERE StatusID = 3) ConfirmedOrder,(SELECT Count(1) FROM Inventory(nolock) WHERE Qty <= 10) LowStocks, (SELECT Count(1) FROM Users u(nolock) INNER JOIN UserRoles ur(nolock) on ur.UserId = u.Id WHERE ur.RoleId = 2) TotalCustomer";
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