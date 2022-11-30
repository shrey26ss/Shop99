using Data.Models;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ICategoryService
    {
        Task<IResponse> AddUpdate(RequestBase<Category> category);
        Task<IResponse<IEnumerable<Category>>> GetCategories(RequestBase<SearchItem> request);
        Task<IResponse> Delete(int id);
        Task<IResponse<List<MenuItem>>> GetMenu(Request request); 
        Task<IResponse<IEnumerable<CategoryDDL>>> GetCategoriesDDL();
    }
}
