﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IBrands
    {
        Task<IResponse> AddUpdate(RequestBase<Brands> request);
        Task<IResponse<IEnumerable<Brands>>> GetBrands(RequestBase<SearchItem> request);
        Task<IResponse> AddUpdateBrandCategoryMapping(RequestBase<BrandCategoryMapping> request);
        Task<IResponse<IEnumerable<BrandCategoryMapping>>> GetBrandCategoryMapping(RequestBase<SearchItem> request);
        Task<IResponse<IEnumerable<BrandsDDL>>> GetBrandDDL();
        Task<IResponse<IEnumerable<Brands>>> GetTopBrands(int Top);
    }
}
