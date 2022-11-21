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
        Task<Response> AddUpdate(RequestBase<Category> category);
        Task<Response<IEnumerable<Category>>> GetCategories(RequestBase<SearchItem> request);
        Task<Response> Delete(int id);
        Task<Response<List<MenuItem>>> GetMenu(Request request);
    }
}
