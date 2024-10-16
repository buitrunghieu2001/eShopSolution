﻿using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            // tên bảng
            builder.ToTable("Carts");
            // khóa chính
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            // khóa ngoại
            builder.HasOne(x => x.Product)
                .WithMany(x => x.Carts)
                .HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.AppUser)
                .WithMany(x => x.Carts)
                .HasForeignKey(x => x.UserId);
        }
    }
}
