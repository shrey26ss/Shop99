using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IDashboard
    {
        Task<IResponse<DashboardTopBoxCount>> GetDashboardTopBoxCount(Request req);
    }
}
