using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Reviews
{
    public class ReviewCreateRequest
    {
        public int ProductId { get; set; }
        public Guid? UserId { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile? ImagePath { get; set; }
    }
}
