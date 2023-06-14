using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class ProductReview
    {
        public int Id { set; get; }
        public int ProductId { set; get; }
        public Guid? UserId { get; set; }
        public string? Content { set; get; }
        public int Rating { set; get; }
        public int IsApproved { set; get; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdate { get; set; }
        public List<ReviewImage> ReviewImage { get; set; }
    }
}
