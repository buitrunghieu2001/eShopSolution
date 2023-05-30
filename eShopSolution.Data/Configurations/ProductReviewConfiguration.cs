using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Configurations
{
    public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {
            // tên bảng
            builder.ToTable("ProductReviews");
            // khóa chính
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Content).HasMaxLength(400);
            builder.Property(x => x.Email).HasMaxLength(100);
            builder.Property(x => x.PhoneNumber).HasMaxLength(10);
        }
    }
}