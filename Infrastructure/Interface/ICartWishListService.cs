using Data.Models;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ICartWishListService
    {
        Task<IResponse> AddWishList(RequestBase<WishList> wishlist);
        Task<IResponse> AddToCart(RequestBase<CartItem> cartitem);
        Task<IResponse<IEnumerable<CartItems>>> GetCartList(Request req);
    }
}
