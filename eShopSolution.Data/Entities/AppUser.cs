﻿using Microsoft.AspNetCore.Identity;

namespace eShopSolution.Data.Entities
{
    // AppUser sử dụng kiểu Guid làm khóa chính cho bảng người dùng
    public class AppUser:IdentityUser<Guid>
    {
        // thêm các thuộc tính cho User (ghi đè IdentityUser)
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public int Status { get; set; }
        public List<Cart> Carts { get; set; }
        public List<Order> Orders { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
