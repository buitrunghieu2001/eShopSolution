using Azure.Core;
using eShopSolution.Application.Catalog.Cart;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.Carts;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Catalog.Carts
{
    public class CartService : ICartService
    {
        // _context: đối tượng của lớp EShopDbContext
        private readonly EShopDbContext _context;
        // constructor
        public CartService(EShopDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
        }

        public async Task<int> addToCart(int productId, Guid userId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);

            if (cart == null)
            {
                cart = new Data.Entities.Cart()
                {
                    ProductId = productId,
                    Quantity = 1,
                    Price = product.OriginalPrice,
                    UserId = userId,
                    DateCreated = DateTime.Now,
                };
                _context.Carts.Add(cart);
            }
            else
            {
                cart.Quantity = cart.Quantity + 1;
            }
            await _context.SaveChangesAsync();
            var count = await _context.Carts.CountAsync(c => c.UserId == userId);
            return count;
        }

        public async Task<GetCartByUserRequest> getCartByUser(Guid userId, string languageId)
        {
            var query = from c in _context.Carts
                        join p in _context.Products on c.ProductId equals p.Id into pc
                        from p in pc.DefaultIfEmpty()
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId into ppt
                        from pt in ppt.DefaultIfEmpty()
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where pt.LanguageId == languageId && c.UserId == userId
                        select new { p, pt, pic, pi, c };

            var items = await query
                .Select(x => new CartVM()
                    {
                        Id = x.c.Id,
                        ProductId = x.c.ProductId,
                        Quantity = x.c.Quantity,
                        Price = x.c.Price,
                        DateCreated = x.c.DateCreated,
                        ImagePath = x.pi.ImagePath,
                        Name = x.pt.Name
                    }).ToListAsync();

            var data = new GetCartByUserRequest
            {
                UserId = userId,
                Items = items
            };

            return data;
        }

        public async Task<int> removeFromCart(int cartId, Guid userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == cartId && c.UserId == userId);
            if (cart == null) throw new EShopException($"Cannot find a cart: {cartId}");
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            var count = await _context.Carts.CountAsync(c => c.UserId == userId);
            return count;
        }

        public async Task<int> updateQuantity(int cartId, int quantity)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null) throw new EShopException($"Cannot find a cart: {cartId}");

            cart.Quantity = quantity; 

            return await _context.SaveChangesAsync();
        }
    }
}
