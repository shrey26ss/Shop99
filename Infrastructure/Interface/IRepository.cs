using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<IResponse<T>> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(T entity = null, int loginId = 0);
        Task<IResponse> AddAsync(T entity);
        Task<IResponse> DeleteAsync(int id);
        Task<IReadOnlyList<T>> GetDropdownAsync(T entity);
    }
}
