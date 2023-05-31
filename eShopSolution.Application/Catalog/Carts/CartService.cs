using Azure.Core;
using eShopSolution.Application.Catalog.Cart;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Carts;
using eShopSolution.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Carts
{
    public class CartService : ICartService
    {
        // _context: đối tượng của lớp EShopDbContext
        private readonly EShopDbContext _context;
        // constructor
        public CartService(EShopDbContext context)
        {
            _context = context;
        }
        public Task<List<CartVM>> addToCart(int productId, Guid userId)
        {
            var cart = new CartVM()
            {
                ProductId = productId,
                UserId = userId,
                Quantity = 1,
                Price = 1,
                DateCreated = DateTime.Now,
            };
            throw new NotImplementedException();
        }

        public Task<List<CartVM>> GetAll(Guid UserId, string languageId)
        {
            throw new NotImplementedException();
        }
    }
}
