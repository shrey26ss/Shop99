using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ICategory
    {
        Task<Response> AddUpdate(CommonRequest<Category> category);
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategoryById(int id);
        Task<Response> Delete(int id);
    }
}
