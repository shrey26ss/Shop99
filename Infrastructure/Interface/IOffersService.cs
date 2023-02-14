﻿using Data.Models;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IOffersService
    {
        Task<IResponse> AddUpdateOffer(RequestBase<GetOffers> request);
        Task<IResponse<IEnumerable<GetOffers>>> Getoffers(RequestBase<SearchItem> request);
        Task<IResponse<IEnumerable<offerstype>>> GetOfferDDL();
        Task<IResponse> UpdateIsActiveOffer(RequestBase<OfferUpdateIsActive> Offer);
    }
}
