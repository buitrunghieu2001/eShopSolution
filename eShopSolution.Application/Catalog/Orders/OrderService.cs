using Azure.Core;
using eShopSolution.Application.Catalog.Cart;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModels.Catalog.Orders;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Catalog.Orders
{
    public class OrderService : IOrderService
    {
        private readonly EShopDbContext _context;
        // constructor
        public OrderService(EShopDbContext context)
        {
            _context = context;
        }


        public async Task<ApiResult<string>> Create(OrderCreateRequest request)
        {
            var carts = await _context.Carts.Where(c => c.UserId == request.UserId).ToListAsync();

            if (carts.Any())
            {
                var orderDetails = new List<OrderDetail>();
                foreach (var item in carts)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                    };
                    orderDetails.Add(orderDetail);
                }

                var order = new Order()
                {
                    OrderDate = DateTime.Now,
                    UserId = request.UserId ?? Guid.Empty,
                    ShipAddress = request.ShipAddress,
                    ShipName = request.ShipName,
                    ShipEmail = request.ShipEmail,
                    ShipPhoneNumber = request.ShipPhoneNumber,
                    ShipProvince = request.ShipProvince,
                    ShipDistrict = request.ShipDistrict,
                    ShipCommune = request.ShipCommune,
                    Status = (int)OrderStatus.InProgress,
                    Notes = request.Notes,
                    OrderDetails = orderDetails
                };

                _context.Orders.Add(order);
                if (await _context.SaveChangesAsync() > 0)
                {
                    _context.Carts.RemoveRange(carts);
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<string>("Đặt hàng thành công.");
                }
            } 
            return new ApiErrorResult<string>("Đặt hàng thất bại. Giỏ hàng không có sản phẩm");
        }

        public async Task<ApiResult<OrderDetailVM>> HasUserPurchasedProductAsync(Guid? userId, int productId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderDetails.Any(od => od.ProductId == productId));

            var isFeedback = await _context.ProductReviews
                .FirstOrDefaultAsync(o => o.UserId == userId && o.ProductId == productId);


            if (order != null && isFeedback == null)
            {
                var orderDetail = order.OrderDetails.FirstOrDefault(od => od.ProductId == productId);
                var orderDetailVM = new OrderDetailVM()
                {
                    OrderId = orderDetail.OrderId, // Sử dụng order.OrderId
                    ProductId = orderDetail.ProductId,
                    Quantity = orderDetail.Quantity,
                    Price = orderDetail.Price
                };

                return new ApiSuccessResult<OrderDetailVM>(orderDetailVM);
            }
            
            return new ApiErrorResult<OrderDetailVM>();
        }

    }
}
