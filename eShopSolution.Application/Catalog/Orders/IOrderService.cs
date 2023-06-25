using eShopSolution.ViewModels.Catalog.Orders;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.Application.Catalog.Orders
{
    public interface IOrderService
    {
        Task<ApiResult<string>> Create(OrderCreateRequest request);
    }
}
