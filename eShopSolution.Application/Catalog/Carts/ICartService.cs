using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Carts;
using eShopSolution.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Cart
{
    public interface ICartService
    {
        Task<List<CartVM>> GetAll(Guid UserId, string languageId);
        Task<List<CartVM>> addToCart(int id, string languageId);
    }
}
