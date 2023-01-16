using Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class ApplicationUser : ApplicationUserProcModel
    {
        public decimal Balance { get; set; }
        public Role RoleId { get; set; }
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
        public string Token { get; set; }
        public string OTP { get; set; }

    }

    public class UserUpdateRequest
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Name { get; set; }


        public AuthenticateResponse(ApplicationUser user, string token)
        {
            user = user ?? new ApplicationUser();
            Id = user.Id;
            Username = user.UserName;
            Role = user.Role;
            Name = user.Name;
            RefreshToken = user.RefreshToken;
            Token = token;
        }
    }
}
