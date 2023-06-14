namespace eShopSolution.ViewModels.Catalog.Carts
{
    public class GetCartByUserRequest
    {
        public Guid UserId { get; set; }

        public List<CartVM> Items { get; set; }
    }
}
