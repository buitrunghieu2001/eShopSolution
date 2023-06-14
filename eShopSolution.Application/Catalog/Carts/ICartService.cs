using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Carts;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Cart
{
    public interface ICartService
    {
        Task<GetCartByUserRequest> getCartByUser(Guid UserId, string language);
        Task<int> addToCart(int productId, Guid userId);
        Task<int> removeFromCart(int cartId, Guid userId);
        Task<int> updateQuantity(int cartId, int quantity);
    }
}
