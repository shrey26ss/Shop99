using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IReport : IRepository<ReportRow, ReportColumn>
    {
        Task<IResponse<IEnumerable<Inventory>>> GetInventoryLadgerReport(RequestBase<InventoryRequest> req);
        Task<IResponse<IEnumerable<Inventory>>> GetInventoryReport(RequestBase<InventoryRequest> req);
        Task<IResponse<IEnumerable<NewsLatter>>> GetNewslatter();
    }
}
