﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.System.Users
{
    public class UserDeleteRequest
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
    }
}
