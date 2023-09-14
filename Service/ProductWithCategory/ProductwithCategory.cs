using Data;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Service.ProductWithCategory
{
    public class ProductwithCategory: IProductwithCategory
    {
        private IDapperRepository _dapper;
        public ProductwithCategory(IDapperRepository dapper)
        {
            _dapper = dapper;
        }
        public async Task<ProductWithCategoryVW> ProductWithCategoryList()
        {
            var response = new ProductWithCategoryVW
            {
                Products = new List<ProductsColumn>(),
                Category = new List<Category>(),
            };
            string query = @"select Top(10)* from Category Where IsPublish = 1;
                            select Top(15)p.*,v.Thumbnail ProductImage,v.MRP,v.SellingCost From Products p inner join VariantGroup v on p.Id = v.ProductId Where p.IsPublished = 1";
            try
            {
                var ProductandCategory = await _dapper.GetMultipleAsync<Category, ProductsColumn>(query, null, CommandType.Text);
                response.Category = (List<Category>)ProductandCategory.GetType().GetProperty("Table1").GetValue(ProductandCategory, null);
                response.Products = (List<ProductsColumn>)ProductandCategory.GetType().GetProperty("Table2").GetValue(ProductandCategory, null);
            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}
