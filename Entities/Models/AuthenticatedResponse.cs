using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class AuthenticatedResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }

    public class TokenApiModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }

    public class RefreshTokenModel
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationTime { get; set; }
    }
    public class LoginAPIRequest
    {
        public string? MerchantID { get; set; }
        public string? SecurityCode { get; set; }
    }
}
