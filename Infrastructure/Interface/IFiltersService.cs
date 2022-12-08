using Data.Models;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IFiltersService
    {
        Task<IResponse<IEnumerable<Filters>>> GetFiltersByCategory(int CategoryId);
    }
}
