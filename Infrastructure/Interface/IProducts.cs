using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IProducts
    {
        Task<Response> AddUpdate(RequestBase<Products> request);
        Task<Response<IEnumerable<Products>>> GetProducts(RequestBase<SearchItem> request);
        Task<Response> AddProductVariant(RequestBase<VariantCombination> request);
    }
}
