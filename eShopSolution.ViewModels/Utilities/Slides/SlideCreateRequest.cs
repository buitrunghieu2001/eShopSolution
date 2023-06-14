using eShopSolution.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace eShopSolution.ViewModels.Utilities.Slides
{
    public class SlideCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
        public Status Status { get; set; }
        public IFormFile? Image { get; set; }

    }
}
