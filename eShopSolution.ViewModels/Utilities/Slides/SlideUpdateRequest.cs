using eShopSolution.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Utilities.Slides
{
    public class SlideUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public IFormFile? Image { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
        public Status Status { get; set; }
    }
}
