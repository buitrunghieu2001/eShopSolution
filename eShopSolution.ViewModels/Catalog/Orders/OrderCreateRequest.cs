using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;

namespace eShopSolution.ViewModels.Catalog.Orders
{
    public class OrderCreateRequest
    {
        public DateTime? OrderDate { set; get; }
        public Guid? UserId { set; get; }
        public string ShipName { set; get; }
        public string ShipAddress { set; get; }
        public string ShipEmail { set; get; }
        public string ShipPhoneNumber { set; get; }
        public string ShipProvince { get; set; }
        public string ShipDistrict { get; set; }
        public string ShipCommune { get; set; }
        public string? Notes { get; set; }
        public OrderStatus? Status { set; get; }
    }
}
