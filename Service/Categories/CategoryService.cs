using Entities.Models;
using Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Categories
{
    public class CategoryService : ICategory
    {
        public Task<Response> AddUpdate(CommonRequest<Category> category)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
