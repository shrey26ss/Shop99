using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Identity
{
    public class ApplicationUser : ApplicationUserProcModel
    {
        public decimal Balance { get; set; }
        public decimal RemainTarget { get; set; }
        public bool OnlyDebtCustomer { get; set; }
        public string FOS { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsActive { get; set; }
    }


    public class ApplicationUserProcModel : IdentityUser<int>
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string GAuthPin { get; set; }
        public int FOSId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

    }

    public class UserUpdateRequest
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
