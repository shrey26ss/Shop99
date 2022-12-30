using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IReport : IRepository<ReportRow, ReportColumn>
    {
        Task<IResponse<IEnumerable<Inventory>>> GetInventoryReport(Request req);
    }
}
