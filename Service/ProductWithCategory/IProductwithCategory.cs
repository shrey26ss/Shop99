using Entities.Models;
using System.Threading.Tasks;

namespace Service.ProductWithCategory
{
    public interface IProductwithCategory
    {
        Task<ProductWithCategoryVW> ProductWithCategoryList();
    }
}
