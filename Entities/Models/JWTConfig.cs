using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class JWTConfig
    {
        public string Secretkey { get; set; }
        public bool IsDevelopmentEnv { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int DurationInMinutes { get; set; }
        public int RefreshTokenExpiry { get; set; }
    }

    public struct ClaimTypesExtension
    {
        public const string Id = "id";
        public const string Role = "role";
        public const string UserName = "userName";
    }
}
